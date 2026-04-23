using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AUT2Services.Tests.Security;

[TestClass]
public class ModificaCorreoElectronicoCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_WhenEmailChanges_RevokesSessionAndRequiresNewConfirmation()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenEmailChanges_RevokesSessionAndRequiresNewConfirmation));
        var usuario = await CreateUserAsync(identityHost, "old-email@example.com");
        var previousSecurityStamp = await identityHost.UserManager.GetSecurityStampAsync(usuario);
        var tokenService = new StubTokenService();
        var authCookieService = new StubAuthCookieService();
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-14T10:00:00Z")));

        var handler = CreateHandler(
            identityHost,
            notificationOutboxService,
            tokenService,
            authCookieService,
            usuario.Id);

        var response = await handler.Handle(CreateCommand(usuario.Id, "new-email@example.com"), CancellationToken.None);
        var payload = response.GetData<EmailConfirmationDispatchResponse>();

        Assert.IsTrue(response.Result);
        Assert.IsNotNull(payload);
        Assert.AreEqual("new-email@example.com", payload!.Email);
        Assert.AreEqual("Correo electronico actualizado. Debes validar el nuevo correo para volver a ingresar a la plataforma.", payload.Message);
        Assert.AreEqual("validation-token", payload.ValidationToken);

        var refreshedUser = await identityHost.UserManager.FindByIdAsync(usuario.Id);
        Assert.IsNotNull(refreshedUser);
        Assert.AreEqual("new-email@example.com", refreshedUser!.Email);
        Assert.AreEqual("new-email@example.com", refreshedUser.UserName);
        Assert.IsFalse(refreshedUser.EmailConfirmed);

        var newSecurityStamp = await identityHost.UserManager.GetSecurityStampAsync(refreshedUser);
        Assert.AreNotEqual(previousSecurityStamp, newSecurityStamp);
        Assert.IsNull(await identityHost.UserManager.FindByEmailAsync("old-email@example.com"));
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count(message =>
            message.NotificationType == NotificationOutboxNotificationTypes.EmailConfirmation));
        Assert.AreEqual(2, identityHost.DbContext.AuditOutboxMessages.Count());
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "CorreoElectronicoModificado"));
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "CorreoValidacionCambioCorreoProgramado"));
        Assert.AreEqual(1, tokenService.RevokedSessions.Count);
        Assert.AreEqual("test-session-id", tokenService.RevokedSessions[0].SessionId);
        Assert.AreEqual(EnumRefreshTokenRevocationReasons.Logout, tokenService.RevokedSessions[0].Reason);
        Assert.IsTrue(authCookieService.Cleared);
    }

    [TestMethod]
    public async Task Handle_WhenEmailMatchesCurrentEmail_ReturnsValidationError()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenEmailMatchesCurrentEmail_ReturnsValidationError));
        var usuario = await CreateUserAsync(identityHost, "same-email@example.com");
        var tokenService = new StubTokenService();
        var authCookieService = new StubAuthCookieService();
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-14T10:00:00Z")));

        var handler = CreateHandler(
            identityHost,
            notificationOutboxService,
            tokenService,
            authCookieService,
            usuario.Id);

        var response = await handler.Handle(CreateCommand(usuario.Id, "same-email@example.com"), CancellationToken.None);

        Assert.IsFalse(response.Result);
        Assert.IsTrue(response.ValidationResult.Errors.Any(error => error.ErrorMessage.Contains("distinto al correo actual")));
        Assert.AreEqual(0, identityHost.DbContext.NotificationOutboxMessages.Count());
        Assert.AreEqual(0, tokenService.RevokedSessions.Count);
        Assert.IsFalse(authCookieService.Cleared);
    }

    private static ModificaCorreoElectronicoCommandHandler CreateHandler(
        IdentityTestHost identityHost,
        NotificationOutboxService notificationOutboxService,
        StubTokenService tokenService,
        StubAuthCookieService authCookieService,
        string userId)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Sid, "test-session-id"),
            new Claim(EnumTokenValidationClaims.SecurityStamp, "security-stamp")
        ], "TestAuth"));

        return new ModificaCorreoElectronicoCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            new StubEmailConfirmationMessageService(),
            new StubCurrentUserService(userId: userId, principal: principal),
            new StubSessionValidationService(),
            tokenService,
            authCookieService,
            NullLogger<ModificaCorreoElectronicoCommandHandler>.Instance);
    }

    private static ModificaCorreoElectronicoCommand CreateCommand(string userId, string newEmail)
    {
        var httpContext = new DefaultHttpContext();

        return new ModificaCorreoElectronicoCommand
        {
            IdUsuario = userId,
            NuevoCorreoElectronico = newEmail,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static async Task<Usuario> CreateUserAsync(IdentityTestHost host, string email)
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

        var result = await host.UserManager.CreateAsync(user, "Changeme123#");
        Assert.IsTrue(result.Succeeded);

        var createdUser = await host.UserManager.FindByEmailAsync(email);
        Assert.IsNotNull(createdUser);
        return createdUser!;
    }
}
