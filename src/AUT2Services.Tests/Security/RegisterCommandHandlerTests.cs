using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.Register;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AUT2Services.Tests.Security;

[TestClass]
public class RegisterCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_WhenEmailDispatchFails_ReturnsFallbackMessageAndPersistsUser()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenEmailDispatchFails_ReturnsFallbackMessageAndPersistsUser));
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-13T12:00:00Z")));
        var handler = new RegisterCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            Options.Create(new JwtOptions { MaximaCantidadIntentosFallidos = 5 }),
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            new StubEmailConfirmationMessageService(),
            NullLogger<RegisterCommandHandler>.Instance);

        var command = CreateRegisterCommand("mail-failure@example.com", "NACIONAL");

        var response = await handler.Handle(command, CancellationToken.None);

        var payload = response.GetData<RegisterResponse>();

        Assert.IsTrue(response.Result);
        Assert.IsNotNull(payload);
        Assert.AreEqual("Usuario registrado. Se programo el envio del correo electronico para confirmar la cuenta.", payload!.Message);
        Assert.IsNotNull(await identityHost.UserManager.FindByEmailAsync("mail-failure@example.com"));
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count());
        Assert.AreEqual(2, identityHost.DbContext.AuditOutboxMessages.Count());
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "UsuarioRegistrado"));
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "CorreoValidacionCuentaProgramado"));
    }

    [TestMethod]
    public async Task Handle_WhenZendeskDispatchFails_DoesNotBreakForeignRegistration()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenZendeskDispatchFails_DoesNotBreakForeignRegistration));
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-13T12:00:00Z")));
        var handler = new RegisterCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            Options.Create(new JwtOptions { MaximaCantidadIntentosFallidos = 5 }),
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            new StubEmailConfirmationMessageService(),
            NullLogger<RegisterCommandHandler>.Instance);

        var command = CreateRegisterCommand("foreign@example.com", "EXTRANJERO");

        var response = await handler.Handle(command, CancellationToken.None);
        var payload = response.GetData<RegisterResponse>();

        Assert.IsTrue(response.Result);
        Assert.IsNotNull(payload);
        Assert.AreEqual("Usuario registrado. Se programo el envio del correo electronico para confirmar la cuenta.", payload!.Message);
        Assert.IsNotNull(await identityHost.UserManager.FindByEmailAsync("foreign@example.com"));
        Assert.AreEqual(2, identityHost.DbContext.NotificationOutboxMessages.Count());
        Assert.AreEqual(3, identityHost.DbContext.AuditOutboxMessages.Count());
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "ZendeskRegistroExtranjeroProgramado"));
    }

    [TestMethod]
    public async Task Handle_WhenValidationTokenIsLong_TruncatesDeduplicationKeyWithoutBreakingRegistration()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenValidationTokenIsLong_TruncatesDeduplicationKeyWithoutBreakingRegistration));
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-13T12:00:00Z")));
        var longTokenService = new LongValidationTokenEmailConfirmationMessageService();
        var handler = new RegisterCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            Options.Create(new JwtOptions { MaximaCantidadIntentosFallidos = 5 }),
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            longTokenService,
            NullLogger<RegisterCommandHandler>.Instance);

        var command = CreateRegisterCommand("long-token@example.com", "NACIONAL");

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(response.Result);
        var queuedMessage = identityHost.DbContext.NotificationOutboxMessages.Single();
        Assert.IsNotNull(queuedMessage.DeduplicationKey);
        Assert.IsTrue(queuedMessage.DeduplicationKey!.Length <= 300);
        Assert.IsTrue(queuedMessage.DeduplicationKey.StartsWith("email-confirmation:", StringComparison.Ordinal));

        var emailPayload = JsonSerializer.Deserialize<EmailDataModel>(
            queuedMessage.PayloadJson,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.IsNotNull(emailPayload);
        Assert.AreEqual(longTokenService.ValidationToken, emailPayload!.ValidationToken);
    }

    private static RegisterCommand CreateRegisterCommand(string email, string tipoDeUsuario)
    {
        var httpContext = new DefaultHttpContext();

        return new RegisterCommand
        {
            CorreoElectronico = email,
            Contraseña = "Changeme123#",
            ConfirmaContraseña = "Changeme123#",
            PrimerNombre = "Test",
            PrimerApellido = "User",
            TipoDeUsuario = tipoDeUsuario,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private sealed class LongValidationTokenEmailConfirmationMessageService : IEmailConfirmationMessageService
    {
        public string ValidationToken { get; } = new('A', 512);

        public Task<EmailConfirmationDispatch> CreateDispatchAsync(Usuario usuario)
        {
            return Task.FromResult(new EmailConfirmationDispatch(
                usuario.Email ?? string.Empty,
                "encoded-user",
                "encoded-token",
                ValidationToken,
                "http://localhost/auto",
                "http://localhost/manual"));
        }

        public EmailDataModel BuildEmailMessage(Usuario usuario, EmailConfirmationDispatch dispatch)
        {
            return new EmailDataModel
            {
                Subject = "Validacion",
                Body = "Body",
                ValidationToken = dispatch.ValidationToken,
                FromMailboxAddresses = [new AddressMailbox { Address = "sender@example.com", Name = "Sender" }],
                ToMailboxAddresses = [new AddressMailbox { Address = usuario.Email ?? string.Empty, Name = usuario.NombreADesplegar ?? string.Empty }]
            };
        }

        public string? GetValidationTokenForResponse(EmailConfirmationDispatch dispatch)
        {
            return dispatch.ValidationToken;
        }
    }
}
