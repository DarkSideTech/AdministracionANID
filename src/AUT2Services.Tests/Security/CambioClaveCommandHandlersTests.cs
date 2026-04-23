using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;
using AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AUT2Services.Tests.Security;

[TestClass]
public class CambioClaveCommandHandlersTests
{
    [TestMethod]
    public async Task SolicitaCambioClave_WhenCurrentPasswordIsValid_CreatesChallengeAndQueuesEmail()
    {
        using var identityHost = new IdentityTestHost(nameof(SolicitaCambioClave_WhenCurrentPasswordIsValid_CreatesChallengeAndQueuesEmail));
        var usuario = await CreateUserAsync(identityHost, "password-request@example.com", "OldPass1!");
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")));
        var handler = new SolicitaCambioClaveCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            CreateCurrentUserService(usuario.Id),
            new StubSessionValidationService(),
            new StubPasswordChangeChallengeMessageService(),
            Options.Create(new PasswordChangeOptions()),
            new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")),
            NullLogger<SolicitaCambioClaveCommandHandler>.Instance);

        var response = await handler.Handle(CreateSolicitaCommand(usuario.Id, "OldPass1!", "NewPass1!"), CancellationToken.None);
        var payload = response.GetData<PasswordChangeChallengeDispatchResponse>();

        Assert.IsTrue(response.Result, string.Join(" | ", response.ValidationResult.Errors.Select(error => error.ErrorMessage)));
        Assert.IsNotNull(payload);
        Assert.AreEqual("password-request@example.com", payload!.Email);
        Assert.AreEqual(1, identityHost.DbContext.PasswordChangeChallenges.Count());
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count());
        Assert.AreEqual(
            1,
            identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "CodigoValidacionCambioClaveProgramado"),
            string.Join(", ", identityHost.DbContext.AuditOutboxMessages.Select(message => $"{message.EventType}:{message.CommandType}")));
        Assert.IsTrue(await identityHost.UserManager.CheckPasswordAsync(usuario, "OldPass1!"));
        Assert.IsFalse(await identityHost.UserManager.CheckPasswordAsync(usuario, "NewPass1!"));
    }

    [TestMethod]
    public async Task ReenviaCodigoCambioClave_WhenActiveChallengeExists_ReplacesChallenge()
    {
        using var identityHost = new IdentityTestHost(nameof(ReenviaCodigoCambioClave_WhenActiveChallengeExists_ReplacesChallenge));
        var usuario = await CreateUserAsync(identityHost, "password-resend@example.com", "OldPass1!");
        identityHost.DbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
        {
            Id = Guid.NewGuid(),
            UserId = usuario.Id,
            CodeHash = "OLD_HASH",
            CreatedAtUtc = DateTimeOffset.Parse("2026-04-16T09:55:00Z"),
            LastSentAtUtc = DateTimeOffset.Parse("2026-04-16T09:55:00Z"),
            ExpiresAtUtc = DateTimeOffset.Parse("2026-04-16T10:30:00Z"),
            FailedAttempts = 0,
            ResendCount = 0
        });
        await identityHost.DbContext.SaveChangesAsync();

        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")));
        var handler = new ReenviaCodigoCambioClaveCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            CreateCurrentUserService(usuario.Id),
            new StubSessionValidationService(),
            new StubPasswordChangeChallengeMessageService(),
            Options.Create(new PasswordChangeOptions { ResendCooldownSeconds = 0 }),
            new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")),
            NullLogger<ReenviaCodigoCambioClaveCommandHandler>.Instance);

        var response = await handler.Handle(CreateReenviaCommand(usuario.Id), CancellationToken.None);

        Assert.IsTrue(response.Result, string.Join(" | ", response.ValidationResult.Errors.Select(error => error.ErrorMessage)));
        Assert.AreEqual(2, identityHost.DbContext.PasswordChangeChallenges.Count());
        Assert.AreEqual(1, identityHost.DbContext.PasswordChangeChallenges.Count(item => item.CancelledAtUtc != null));
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count());
        var resendAuditEvents = string.Join(", ", identityHost.DbContext.AuditOutboxMessages.Select(message => $"{message.EventType}:{message.CommandType}"));
        if (identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "CodigoValidacionCambioClaveProgramado") != 1)
        {
            Assert.Fail($"Eventos de auditoria registrados: {resendAuditEvents}");
        }
    }

    [TestMethod]
    public async Task ConfirmaCambioClave_WhenCodeMatches_ChangesPasswordAndConsumesChallenge()
    {
        using var identityHost = new IdentityTestHost(nameof(ConfirmaCambioClave_WhenCodeMatches_ChangesPasswordAndConsumesChallenge));
        var usuario = await CreateUserAsync(identityHost, "password-confirm@example.com", "OldPass1!");
        identityHost.DbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
        {
            Id = Guid.NewGuid(),
            UserId = usuario.Id,
            CodeHash = "TEST_HASH",
            CreatedAtUtc = DateTimeOffset.Parse("2026-04-16T09:55:00Z"),
            LastSentAtUtc = DateTimeOffset.Parse("2026-04-16T09:55:00Z"),
            ExpiresAtUtc = DateTimeOffset.Parse("2026-04-16T10:30:00Z"),
            FailedAttempts = 0,
            ResendCount = 0
        });
        await identityHost.DbContext.SaveChangesAsync();

        var tokenService = new StubTokenService();
        var authCookieService = new StubAuthCookieService();
        var handler = new ConfirmaCambioClaveCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            new StubCsrfService(),
            CreateCurrentUserService(usuario.Id),
            new StubSessionValidationService(),
            new StubPasswordChangeChallengeMessageService(),
            tokenService,
            authCookieService,
            identityHost.SecurityTraceabilityService,
            Options.Create(new PasswordChangeOptions()),
            new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")),
            NullLogger<ConfirmaCambioClaveCommandHandler>.Instance);

        var response = await handler.Handle(CreateConfirmaCommand(usuario.Id, "OldPass1!", "NewPass1!", "123456"), CancellationToken.None);
        var updatedUser = await identityHost.UserManager.FindByIdAsync(usuario.Id);

        Assert.IsTrue(response.Result);
        Assert.IsNotNull(updatedUser);
        Assert.IsTrue(await identityHost.UserManager.CheckPasswordAsync(updatedUser!, "NewPass1!"));
        Assert.IsFalse(await identityHost.UserManager.CheckPasswordAsync(updatedUser!, "OldPass1!"));
        Assert.AreEqual(1, tokenService.RevokedSessions.Count);
        Assert.IsTrue(authCookieService.Cleared);
        Assert.AreEqual(1, identityHost.DbContext.PasswordChangeChallenges.Count(item => item.ConsumedAtUtc != null));
        var confirmAuditEvents = string.Join(", ", identityHost.DbContext.AuditOutboxMessages.Select(message => $"{message.EventType}:{message.CommandType}"));
        if (identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "ValidacionCambioClaveRespondida") != 1)
        {
            Assert.Fail($"Eventos de auditoria registrados: {confirmAuditEvents}");
        }

        if (identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "ClaveAccesoModificada") != 1)
        {
            Assert.Fail($"Eventos de auditoria registrados: {confirmAuditEvents}");
        }
    }

    private static StubCurrentUserService CreateCurrentUserService(string userId)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Sid, "test-session-id"),
            new Claim("sst", "security-stamp")
        ], "TestAuth"));

        return new StubCurrentUserService(userId: userId, principal: principal);
    }

    private static SolicitaCambioClaveCommand CreateSolicitaCommand(string userId, string currentPassword, string newPassword)
    {
        var httpContext = new DefaultHttpContext();
        return new SolicitaCambioClaveCommand
        {
            IdUsuario = userId,
            ClaveActual = currentPassword,
            NuevaClave = newPassword,
            ConfirmaNuevaClave = newPassword,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static ReenviaCodigoCambioClaveCommand CreateReenviaCommand(string userId)
    {
        var httpContext = new DefaultHttpContext();
        return new ReenviaCodigoCambioClaveCommand
        {
            IdUsuario = userId,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static ConfirmaCambioClaveCommand CreateConfirmaCommand(string userId, string currentPassword, string newPassword, string code)
    {
        var httpContext = new DefaultHttpContext();
        return new ConfirmaCambioClaveCommand
        {
            IdUsuario = userId,
            ClaveActual = currentPassword,
            NuevaClave = newPassword,
            ConfirmaNuevaClave = newPassword,
            CodigoValidacion = code,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static async Task<Usuario> CreateUserAsync(IdentityTestHost host, string email, string password)
    {
        var user = new Usuario
        {
            UserName = email,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            EmailConfirmed = true,
            PhoneNumber = "+56911111111",
            NombreADesplegar = "Nombre Inicial",
            TipoDeUsuario = EnumTipoDeUsuario.EXTRANJERO,
            Activo = true,
            UsuarioBase = false,
            EstadoDeUsuario = EnumEstadoDeUsuario.REGISTRADO,
            InformacionAdicional = new InformacionAdicionalModel
            {
                Nacionalidad = EnumNacionalidad.Chileno_a,
                DocumentoDeIdentidad = "PASAPORTE",
                NumeroDeDocumento = "11111111",
                CodigoValidadorDocumento = "1",
                PrimerNombre = "Nombre",
                SegundoNombre = "Segundo",
                PrimerApellido = "Apellido",
                SegundoApellido = "SegundoApellido",
                SexoDeclarativo = "MUJER",
                SexoRegistral = "FEMENINO",
                FechaDeNacimiento = new DateOnly(1990, 1, 1),
                TerminosYCondiciones = true
            }.ToJson()
        };

        var result = await host.UserManager.CreateAsync(user, password);
        Assert.IsTrue(result.Succeeded);

        var createdUser = await host.UserManager.FindByEmailAsync(email);
        Assert.IsNotNull(createdUser);
        return createdUser!;
    }
}
