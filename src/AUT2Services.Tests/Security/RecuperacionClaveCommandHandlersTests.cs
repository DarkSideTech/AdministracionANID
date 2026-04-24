using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace AUT2Services.Tests.Security;

[TestClass]
public class RecuperacionClaveCommandHandlersTests
{
    [TestMethod]
    public async Task SolicitaRecuperacionClave_WhenUserExists_CreatesRecoveryChallengeAndQueuesEmail()
    {
        using var identityHost = new IdentityTestHost(nameof(SolicitaRecuperacionClave_WhenUserExists_CreatesRecoveryChallengeAndQueuesEmail));
        var usuario = await CreateUserAsync(identityHost, "recovery-request@example.com", "OldPass1!");
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")));
        var handler = new SolicitaRecuperacionClaveCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            new StubPasswordChangeChallengeMessageService(),
            Options.Create(new PasswordChangeOptions()),
            new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")),
            NullLogger<SolicitaRecuperacionClaveCommandHandler>.Instance);

        var response = await handler.Handle(CreateSolicitaCommand(usuario.Email!), CancellationToken.None);
        var payload = response.GetData<PasswordChangeChallengeDispatchResponse>();

        Assert.IsTrue(response.Result, string.Join(" | ", response.ValidationResult.Errors.Select(error => error.ErrorMessage)));
        Assert.IsNotNull(payload);
        Assert.AreEqual("recovery-request@example.com", payload!.Email);
        Assert.AreEqual(1, identityHost.DbContext.PasswordChangeChallenges.Count(item => item.ChallengePurpose == PasswordChangeChallengePurposes.PasswordRecovery));
        Assert.AreEqual(1, identityHost.DbContext.NotificationOutboxMessages.Count(message => message.NotificationType == NotificationOutboxNotificationTypes.PasswordRecoveryVerificationCode));
        Assert.AreEqual(
            1,
            identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "CodigoValidacionRecuperacionClaveProgramado"),
            string.Join(", ", identityHost.DbContext.AuditOutboxMessages.Select(message => $"{message.EventType}:{message.CommandType}")));
        Assert.IsTrue(await identityHost.UserManager.CheckPasswordAsync(usuario, "OldPass1!"));
    }

    [TestMethod]
    public async Task SolicitaRecuperacionClave_WhenUserDoesNotExist_ReturnsNeutralResponseWithoutQueueingEmail()
    {
        using var identityHost = new IdentityTestHost(nameof(SolicitaRecuperacionClave_WhenUserDoesNotExist_ReturnsNeutralResponseWithoutQueueingEmail));
        var notificationOutboxService = new NotificationOutboxService(identityHost.DbContext, new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")));
        var handler = new SolicitaRecuperacionClaveCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            notificationOutboxService,
            identityHost.SecurityTraceabilityService,
            new StubCsrfService(),
            new StubPasswordChangeChallengeMessageService(),
            Options.Create(new PasswordChangeOptions()),
            new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")),
            NullLogger<SolicitaRecuperacionClaveCommandHandler>.Instance);

        var response = await handler.Handle(CreateSolicitaCommand("missing-recovery@example.com"), CancellationToken.None);

        Assert.IsTrue(response.Result, string.Join(" | ", response.ValidationResult.Errors.Select(error => error.ErrorMessage)));
        Assert.AreEqual(0, identityHost.DbContext.PasswordChangeChallenges.Count());
        Assert.AreEqual(0, identityHost.DbContext.NotificationOutboxMessages.Count());
    }

    [TestMethod]
    public async Task ConfirmaRecuperacionClave_WhenCodeMatches_ChangesPasswordConsumesChallengeAndRevokesSessions()
    {
        using var identityHost = new IdentityTestHost(nameof(ConfirmaRecuperacionClave_WhenCodeMatches_ChangesPasswordConsumesChallengeAndRevokesSessions));
        var usuario = await CreateUserAsync(identityHost, "recovery-confirm@example.com", "OldPass1!");
        identityHost.DbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
        {
            Id = Guid.NewGuid(),
            UserId = usuario.Id,
            ChallengePurpose = PasswordChangeChallengePurposes.PasswordRecovery,
            CodeHash = "TEST_HASH",
            CreatedAtUtc = DateTimeOffset.Parse("2026-04-16T09:55:00Z"),
            LastSentAtUtc = DateTimeOffset.Parse("2026-04-16T09:55:00Z"),
            ExpiresAtUtc = DateTimeOffset.Parse("2026-04-16T10:30:00Z"),
            FailedAttempts = 0,
            ResendCount = 0
        });
        identityHost.DbContext.RefreshTokens.Add(new RefreshToken
        {
            SessionId = "session-recovery",
            TokenHash = "TOKEN_HASH",
            UserId = usuario.Id,
            Id_Entidad = Guid.NewGuid(),
            CreatedAtUtc = DateTimeOffset.Parse("2026-04-16T09:00:00Z"),
            ExpiresAtUtc = DateTimeOffset.Parse("2026-04-17T09:00:00Z")
        });
        await identityHost.DbContext.SaveChangesAsync();
        identityHost.DbContext.ChangeTracker.Clear();

        var handler = new ConfirmaRecuperacionClaveCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            new StubCsrfService(),
            new StubPasswordChangeChallengeMessageService(),
            identityHost.SecurityTraceabilityService,
            Options.Create(new PasswordChangeOptions()),
            new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z")),
            NullLogger<ConfirmaRecuperacionClaveCommandHandler>.Instance);

        var response = await handler.Handle(CreateConfirmaCommand(usuario.Email!, "NewPass1!", "123456"), CancellationToken.None);
        var updatedUser = await identityHost.UserManager.FindByIdAsync(usuario.Id);
        var revokedToken = identityHost.DbContext.RefreshTokens.Single();

        Assert.IsTrue(response.Result, string.Join(" | ", response.ValidationResult.Errors.Select(error => error.ErrorMessage)));
        Assert.IsNotNull(updatedUser);
        Assert.IsTrue(await identityHost.UserManager.CheckPasswordAsync(updatedUser!, "NewPass1!"));
        Assert.IsFalse(await identityHost.UserManager.CheckPasswordAsync(updatedUser!, "OldPass1!"));
        Assert.AreEqual(1, identityHost.DbContext.PasswordChangeChallenges.Count(item => item.ConsumedAtUtc != null));
        Assert.IsNotNull(revokedToken.RevokedAtUtc);
        Assert.AreEqual(EnumRefreshTokenRevocationReasons.PasswordRecovery, revokedToken.RevocationReason);
        Assert.AreEqual(1, identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "ValidacionRecuperacionClaveRespondida"));
        Assert.AreEqual(1, identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "ClaveAccesoRecuperada"));
    }

    private static SolicitaRecuperacionClaveCommand CreateSolicitaCommand(string email)
    {
        var httpContext = new DefaultHttpContext();
        return new SolicitaRecuperacionClaveCommand
        {
            CorreoElectronico = email,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static ConfirmaRecuperacionClaveCommand CreateConfirmaCommand(string email, string newPassword, string code)
    {
        var httpContext = new DefaultHttpContext();
        return new ConfirmaRecuperacionClaveCommand
        {
            CorreoElectronico = email,
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
