using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.ResendEmailConfirmationToken;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace AUT2Services.Tests.Security;

[TestClass]
public class ResendEmailConfirmationTokenCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_WhenEmailDispatchFails_AddsValidationError()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenEmailDispatchFails_AddsValidationError));
        await identityHost.UserManager.CreateAsync(new Usuario
        {
            UserName = "pending@example.com",
            Email = "pending@example.com",
            NormalizedEmail = "PENDING@EXAMPLE.COM",
            NormalizedUserName = "PENDING@EXAMPLE.COM",
            NombreADesplegar = "Pending User",
            EmailConfirmed = false
        }, "Changeme123#");
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-13T12:00:00Z")));

        var handler = new ResendEmailConfirmationTokenCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            new StubCsrfService(),
            new StubEmailConfirmationThrottleService(),
            notificationOutboxService,
            new StubEmailConfirmationMessageService(),
            identityHost.SecurityTraceabilityService,
            NullLogger<ResendEmailConfirmationTokenCommandHandler>.Instance);

        var response = await handler.Handle(CreateCommand("pending@example.com"), CancellationToken.None);

        Assert.IsTrue(response.Result);
        Assert.IsTrue(response.ValidationResult.IsValid);
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count());
        Assert.AreEqual(1, identityHost.DbContext.AuditOutboxMessages.Count());
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "CorreoValidacionCuentaProgramado"));
    }

    [TestMethod]
    public async Task Handle_WhenEmailDispatchSucceeds_ReturnsConfirmationPayload()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenEmailDispatchSucceeds_ReturnsConfirmationPayload));
        await identityHost.UserManager.CreateAsync(new Usuario
        {
            UserName = "pending-success@example.com",
            Email = "pending-success@example.com",
            NormalizedEmail = "PENDING-SUCCESS@EXAMPLE.COM",
            NormalizedUserName = "PENDING-SUCCESS@EXAMPLE.COM",
            NombreADesplegar = "Pending User",
            EmailConfirmed = false
        }, "Changeme123#");
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-13T12:00:00Z")));

        var handler = new ResendEmailConfirmationTokenCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            new StubCsrfService(),
            new StubEmailConfirmationThrottleService(),
            notificationOutboxService,
            new StubEmailConfirmationMessageService(),
            identityHost.SecurityTraceabilityService,
            NullLogger<ResendEmailConfirmationTokenCommandHandler>.Instance);

        var response = await handler.Handle(CreateCommand("pending-success@example.com"), CancellationToken.None);
        var payload = response.GetData<EmailConfirmationDispatchResponse>();

        Assert.IsTrue(response.Result);
        Assert.IsTrue(response.ValidationResult.IsValid);
        Assert.IsNotNull(payload);
        Assert.AreEqual("Se ha programado un nuevo correo electronico de confirmacion.", payload!.Message);
        Assert.AreEqual("validation-token", payload.ValidationToken);
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count(message =>
            message.NotificationType == NotificationOutboxNotificationTypes.EmailConfirmation));
        Assert.AreEqual(1, identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "CorreoValidacionCuentaProgramado"));
    }

    private static ResendEmailConfirmationTokenCommand CreateCommand(string email)
    {
        var httpContext = new DefaultHttpContext();

        return new ResendEmailConfirmationTokenCommand
        {
            Email = email,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }
}
