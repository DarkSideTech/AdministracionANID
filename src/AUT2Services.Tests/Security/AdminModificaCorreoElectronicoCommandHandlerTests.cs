using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.AdminModificaCorreoElectronico;
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
public class AdminModificaCorreoElectronicoCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_ForTargetUser_ChangesEmailAndDoesNotRequireOperatorAsTarget()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_ForTargetUser_ChangesEmailAndDoesNotRequireOperatorAsTarget));
        var operador = await CreateUserAsync(identityHost, "operator-email@example.com");
        var usuarioObjetivo = await CreateUserAsync(identityHost, "target-old-email@example.com");
        var previousSecurityStamp = await identityHost.UserManager.GetSecurityStampAsync(usuarioObjetivo);
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-14T10:00:00Z")));

        var handler = CreateHandler(identityHost, notificationOutboxService, operador.Id);

        var response = await handler.Handle(
            CreateCommand(usuarioObjetivo.Id, "target-new-email@example.com"),
            CancellationToken.None);
        var payload = response.GetData<EmailConfirmationDispatchResponse>();

        Assert.IsTrue(response.Result);
        Assert.IsNotNull(payload);
        Assert.AreEqual("target-new-email@example.com", payload!.Email);
        Assert.AreEqual("Correo electronico actualizado. El usuario debe validar el nuevo correo para volver a ingresar a la plataforma.", payload.Message);

        var refreshedUser = await identityHost.UserManager.FindByIdAsync(usuarioObjetivo.Id);
        Assert.IsNotNull(refreshedUser);
        Assert.AreEqual("target-new-email@example.com", refreshedUser!.Email);
        Assert.AreEqual("target-new-email@example.com", refreshedUser.UserName);
        Assert.IsFalse(refreshedUser.EmailConfirmed);

        var newSecurityStamp = await identityHost.UserManager.GetSecurityStampAsync(refreshedUser);
        Assert.AreNotEqual(previousSecurityStamp, newSecurityStamp);
        Assert.AreEqual("operator-email@example.com", (await identityHost.UserManager.FindByIdAsync(operador.Id))!.Email);
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count(message =>
            message.NotificationType == NotificationOutboxNotificationTypes.EmailConfirmation));
        Assert.AreEqual(2, identityHost.DbContext.AuditOutboxMessages.Count());
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "CorreoElectronicoModificado"));
        Assert.IsTrue(identityHost.DbContext.AuditOutboxMessages.Any(message => message.EventType == "CorreoValidacionCambioCorreoProgramado"));
    }

    [TestMethod]
    public async Task Handle_WhenTargetUserHasPendingEmailConfirmation_ReturnsValidationError()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenTargetUserHasPendingEmailConfirmation_ReturnsValidationError));
        var operador = await CreateUserAsync(identityHost, "operator-pending@example.com");
        var usuarioObjetivo = await CreateUserAsync(identityHost, "target-pending@example.com", emailConfirmed: false);
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-14T10:00:00Z")));

        var handler = CreateHandler(identityHost, notificationOutboxService, operador.Id);

        var response = await handler.Handle(
            CreateCommand(usuarioObjetivo.Id, "target-pending-new@example.com"),
            CancellationToken.None);

        Assert.IsFalse(response.Result);
        Assert.IsTrue(response.ValidationResult.Errors.Any(error => error.ErrorMessage.Contains("validacion pendiente")));
        Assert.AreEqual(0, identityHost.DbContext.NotificationOutboxMessages.Count());
        Assert.AreEqual(0, identityHost.DbContext.AuditOutboxMessages.Count());
    }

    private static AdminModificaCorreoElectronicoCommandHandler CreateHandler(
        IdentityTestHost identityHost,
        NotificationOutboxService notificationOutboxService,
        string operatorUserId)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, operatorUserId),
            new Claim(JwtRegisteredClaimNames.Sid, "test-session-id"),
            new Claim(EnumTokenValidationClaims.SecurityStamp, "security-stamp")
        ], "TestAuth"));

        return new AdminModificaCorreoElectronicoCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            new StubEmailConfirmationMessageService(),
            new StubCurrentUserService(userId: operatorUserId, principal: principal),
            new StubSessionValidationService(),
            NullLogger<AdminModificaCorreoElectronicoCommandHandler>.Instance);
    }

    private static AdminModificaCorreoElectronicoCommand CreateCommand(string userId, string newEmail)
    {
        var httpContext = new DefaultHttpContext();

        return new AdminModificaCorreoElectronicoCommand
        {
            IdUsuario = userId,
            NuevoCorreoElectronico = newEmail,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static async Task<Usuario> CreateUserAsync(IdentityTestHost host, string email, bool emailConfirmed = true)
    {
        var user = new Usuario
        {
            UserName = email,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            EmailConfirmed = emailConfirmed,
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
