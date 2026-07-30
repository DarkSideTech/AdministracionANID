using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Accounts.LoginClaveUnica;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace AUT2Services.Tests.Security;

[TestClass]
public class LoginClaveUnicaCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_WhenExistingPersonaIsUniqueAndPrincipal_IssuesSessionWithoutCreatingAnother()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenExistingPersonaIsUniqueAndPrincipal_IssuesSessionWithoutCreatingAnother));
        var usuario = await CreateExistingClaveUnicaUserAsync(identityHost.UserManager);
        var entidadRepository = new InMemoryEntidadRepository([CreatePersona(Guid.Parse(usuario.Id), principal: true)]);
        var mediator = new RecordingMediatorHandler();
        var tokenService = new RecordingTokenService();
        var handler = CreateHandler(identityHost, entidadRepository, mediator, tokenService);

        var response = await handler.Handle(CreateLoginCommand(), CancellationToken.None);

        Assert.IsTrue(response.Result);
        Assert.AreEqual(0, mediator.BaseEntityCommandCount);
        Assert.AreEqual(1, tokenService.AccessTokenGenerationCount);
    }

    [TestMethod]
    public async Task Handle_WhenExistingPersonaIsNotPrincipal_BlocksLoginWithoutCreatingAnother()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenExistingPersonaIsNotPrincipal_BlocksLoginWithoutCreatingAnother));
        var usuario = await CreateExistingClaveUnicaUserAsync(identityHost.UserManager);
        var entidadRepository = new InMemoryEntidadRepository([CreatePersona(Guid.Parse(usuario.Id), principal: false)]);
        var mediator = new RecordingMediatorHandler();
        var tokenService = new RecordingTokenService();
        var handler = CreateHandler(identityHost, entidadRepository, mediator, tokenService);

        var response = await handler.Handle(CreateLoginCommand(), CancellationToken.None);

        Assert.IsFalse(response.Result);
        Assert.AreEqual(0, mediator.BaseEntityCommandCount);
        Assert.AreEqual(0, tokenService.AccessTokenGenerationCount);
    }

    [TestMethod]
    public async Task Handle_WhenMultiplePersonasExist_BlocksLoginWithoutCreatingAnother()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenMultiplePersonasExist_BlocksLoginWithoutCreatingAnother));
        var usuario = await CreateExistingClaveUnicaUserAsync(identityHost.UserManager);
        var userId = Guid.Parse(usuario.Id);
        var entidadRepository = new InMemoryEntidadRepository(
        [
            CreatePersona(userId, principal: true),
            CreatePersona(userId, principal: false)
        ]);
        var mediator = new RecordingMediatorHandler();
        var tokenService = new RecordingTokenService();
        var handler = CreateHandler(identityHost, entidadRepository, mediator, tokenService);

        var response = await handler.Handle(CreateLoginCommand(), CancellationToken.None);

        Assert.IsFalse(response.Result);
        Assert.AreEqual(0, mediator.BaseEntityCommandCount);
        Assert.AreEqual(0, tokenService.AccessTokenGenerationCount);
    }

    [TestMethod]
    public async Task Handle_WhenNewUserHasNoPersona_CreatesAndValidatesUniquePrincipalPersonaBeforeIssuingSession()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenNewUserHasNoPersona_CreatesAndValidatesUniquePrincipalPersonaBeforeIssuingSession));
        var entidadRepository = new InMemoryEntidadRepository([]);
        var mediator = new RecordingMediatorHandler(command => entidadRepository.Crear(CreatePersona(command.Id_Usuario!.Value, principal: true)));
        var tokenService = new RecordingTokenService();
        var handler = CreateHandler(identityHost, entidadRepository, mediator, tokenService);

        var response = await handler.Handle(CreateLoginCommand(), CancellationToken.None);

        Assert.IsTrue(response.Result);
        Assert.AreEqual(1, mediator.BaseEntityCommandCount);
        Assert.AreEqual(1, entidadRepository.Entidades.Count);
        Assert.IsTrue(entidadRepository.Entidades.Single().Principal);
        Assert.AreEqual(1, tokenService.AccessTokenGenerationCount);
    }

    [TestMethod]
    public async Task Handle_WhenBaseEntityCreationFails_DoesNotIssueSession()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenBaseEntityCreationFails_DoesNotIssueSession));
        var entidadRepository = new InMemoryEntidadRepository([]);
        var mediator = new RecordingMediatorHandler(succeeds: false);
        var tokenService = new RecordingTokenService();
        var handler = CreateHandler(identityHost, entidadRepository, mediator, tokenService);

        var response = await handler.Handle(CreateLoginCommand(), CancellationToken.None);

        Assert.IsFalse(response.Result);
        Assert.AreEqual(1, mediator.BaseEntityCommandCount);
        Assert.AreEqual(0, tokenService.AccessTokenGenerationCount);
    }

    [TestMethod]
    public async Task Handle_WhenBaseEntityPostconditionIsInvalid_DoesNotIssueSession()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_WhenBaseEntityPostconditionIsInvalid_DoesNotIssueSession));
        var entidadRepository = new InMemoryEntidadRepository([]);
        var mediator = new RecordingMediatorHandler(command => entidadRepository.Crear(CreatePersona(command.Id_Usuario!.Value, principal: false)));
        var tokenService = new RecordingTokenService();
        var handler = CreateHandler(identityHost, entidadRepository, mediator, tokenService);

        var response = await handler.Handle(CreateLoginCommand(), CancellationToken.None);

        Assert.IsFalse(response.Result);
        Assert.AreEqual(1, mediator.BaseEntityCommandCount);
        Assert.AreEqual(0, tokenService.AccessTokenGenerationCount);
    }

    private static LoginClaveUnicaCommandHandler CreateHandler(
        IdentityTestHost identityHost,
        IEntidadRepository entidadRepository,
        IMediatorHandler mediator,
        ITokenService tokenService)
    {
        return new LoginClaveUnicaCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            tokenService,
            new RecordingAuthCookieService(),
            new StubCsrfService(),
            entidadRepository,
            mediator,
            new TestClaveUnicaClient(),
            identityHost.SecurityTraceabilityService,
            NullLogger<LoginClaveUnicaCommandHandler>.Instance);
    }

    private static LoginClaveUnicaCommand CreateLoginCommand()
    {
        var httpContext = new DefaultHttpContext();

        return new LoginClaveUnicaCommand
        {
            Code = "test-code",
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static async Task<Usuario> CreateExistingClaveUnicaUserAsync(UserManager<Usuario> userManager)
    {
        var email = $"claveunica-test-{Guid.NewGuid():N}@anid.local";
        var usuario = new Usuario
        {
            UserName = email,
            Email = email,
            NormalizedUserName = email.ToUpperInvariant(),
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = true,
            Activo = true
        };

        var creationResult = await userManager.CreateAsync(usuario);
        Assert.IsTrue(creationResult.Succeeded);

        var loginResult = await userManager.AddLoginAsync(usuario, new UserLoginInfo("CLAVEUNICA", "11111111-1", "Clave Única"));
        Assert.IsTrue(loginResult.Succeeded);
        return usuario;
    }

    private static Entidad CreatePersona(Guid userId, bool principal)
    {
        return new Entidad(
            Guid.NewGuid(),
            Guid.Empty,
            userId,
            EnumTipoDeEntidad.PERSONA,
            string.Empty,
            null,
            null,
            DateTimeOffset.UtcNow,
            principal,
            entidadBase: true);
    }

    private sealed class TestClaveUnicaClient : IClaveUnicaClient
    {
        public bool IsConfigured => true;

        public Task<string> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
        {
            return Task.FromResult("test-access-token");
        }

        public Task<ClaveUnicaUserInfoResponse> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ClaveUnicaUserInfoResponse
            {
                Sub = "test-subject",
                RolUnico = new ClaveUnicaRolUnicoPayload
                {
                    Numero = 11111111,
                    Dv = "1",
                    Tipo = EnumDocumentoDeIdentidad.RUN
                },
                Name = new ClaveUnicaNamePayload
                {
                    Nombres = ["Test"],
                    Apellidos = ["User"]
                }
            });
        }
    }

    private sealed class RecordingAuthCookieService : IAuthCookieService
    {
        public void AppendAuthCookies(HttpResponse response, AccessTokenResult accessToken, DateTimeOffset? refreshTokenExpiresAtUtc, string refreshToken)
        {
        }

        public void ClearAuthCookies(HttpResponse response)
        {
        }
    }

    private sealed class RecordingTokenService : ITokenService
    {
        public int AccessTokenGenerationCount { get; private set; }

        public Task<AccessTokenResult> GenerateAccessTokenAsync(Usuario user, string sessionId, Guid? idEntidad = null, Guid? idRol = null)
        {
            AccessTokenGenerationCount++;
            return Task.FromResult(new AccessTokenResult("test-token", DateTimeOffset.UtcNow.AddMinutes(5)));
        }

        public RefreshTokenIssuanceResult CreateRefreshToken(string sessionId, string? selectedOrganization = null)
        {
            return new RefreshTokenIssuanceResult(
                "test-refresh-token",
                new RefreshToken
                {
                    SessionId = sessionId,
                    TokenHash = "test-token-hash",
                    CreatedAtUtc = DateTimeOffset.UtcNow,
                    ExpiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(5),
                    SelectedOrganization = selectedOrganization
                });
        }

        public Task<IList<OrganizacionPorUsuario>> BuscarOrganizacionesPorIdUsuario(string idUsuario)
        {
            return Task.FromResult<IList<OrganizacionPorUsuario>>([]);
        }

        public string HashRefreshToken(string refreshToken)
        {
            return "test-token-hash";
        }

        public Task<UserDto> CreateUserDtoAsync(Usuario user, Guid? id_Entidad = null, EntidadRolSeleccionado? entidadRolSeleccionado = null)
        {
            return Task.FromResult(new UserDto(
                Id: user.Id,
                Email: user.Email,
                NombreADesplegar: user.NombreADesplegar,
                NumeroDeTelefono: user.PhoneNumber,
                TipoDeUsuario: user.TipoDeUsuario,
                Nacionalidad: null,
                DocumentoDeIdentidad: null,
                NumeroDeDocumento: null,
                CodigoValidadorDocumento: null,
                PrimerNombre: null,
                SegundoNombre: null,
                PrimerApellido: null,
                SegundoApellido: null,
                SexoDeclarativo: null,
                SexoRegistral: null,
                FechaDeNacimiento: null,
                Roles: [],
                UnidadesOrganizacionales: []));
        }

        public Task RevokeSessionAsync(string sessionId, string reason)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class RecordingMediatorHandler(Action<BaseEntityCommand>? onBaseEntityCommand = null, bool succeeds = true) : IMediatorHandler
    {
        public int BaseEntityCommandCount { get; private set; }

        public Task PublishEvent<T>(T @event) where T : Event
        {
            return Task.CompletedTask;
        }

        public Task PublishEvent<T>(T @event, CancellationToken cancellationToken) where T : Event
        {
            return Task.CompletedTask;
        }

        public Task<CommandResponse> SendCommand<T>(T command) where T : Command
        {
            return SendCommand(command, CancellationToken.None);
        }

        public Task<CommandResponse> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command
        {
            if (command is BaseEntityCommand baseEntityCommand)
            {
                BaseEntityCommandCount++;
                onBaseEntityCommand?.Invoke(baseEntityCommand);
            }

            return Task.FromResult(new CommandResponse { Result = succeeds });
        }
    }

    private sealed class InMemoryEntidadRepository(IEnumerable<Entidad> entidades) : IEntidadRepository
    {
        public List<Entidad> Entidades { get; } = entidades.ToList();
        public IUnitOfWork UnitOfWork { get; } = new CompletedUnitOfWork();

        public void Crear(Entidad data)
        {
            Entidades.Add(data);
        }

        public void Modificar(Entidad data)
        {
        }

        public void Eliminar(Entidad data)
        {
            Entidades.Remove(data);
        }

        public Task<Entidad> BuscarPor_Id(Guid id)
        {
            return Task.FromResult(Entidades.SingleOrDefault(entidad => entidad.Id == id)!);
        }

        public Task<Entidad> BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal(Guid id_Usuario, Guid id_UnidadOrganizacional)
        {
            return Task.FromResult(Entidades.SingleOrDefault(entidad =>
                entidad.Id_Usuario == id_Usuario
                && entidad.Id_UnidadOrganizacional == id_UnidadOrganizacional
                && entidad.Principal)!);
        }

        public Task<Entidad> BuscarPor_Id_Usuario_Id_UnidadOrganizacional(Guid id_Usuario, Guid id_UnidadOrganizacional)
        {
            return Task.FromResult(Entidades.SingleOrDefault(entidad =>
                entidad.Id_Usuario == id_Usuario
                && entidad.Id_UnidadOrganizacional == id_UnidadOrganizacional)!);
        }

        public Task<IEnumerable<Entidad>> BuscarPor_Id_Usuario(Guid id_Usuario)
        {
            return Task.FromResult<IEnumerable<Entidad>>(Entidades.Where(entidad => entidad.Id_Usuario == id_Usuario).ToList());
        }

        public Task<IEnumerable<Entidad>> BuscarPor_Id_UnidadOrganizacional(Guid id_UnidadOrganizacional)
        {
            return Task.FromResult<IEnumerable<Entidad>>(Entidades.Where(entidad => entidad.Id_UnidadOrganizacional == id_UnidadOrganizacional).ToList());
        }

        public Task<Entidad> BuscarPor_Id_Usuario_TipoDeEntidad_Persona(Guid id_Usuario)
        {
            return Task.FromResult(Entidades.SingleOrDefault(entidad =>
                entidad.Id_Usuario == id_Usuario
                && EnumTipoDeEntidad.PERSONA.Equals(entidad.TipoDeEntidad, StringComparison.OrdinalIgnoreCase)
                && entidad.Principal)!);
        }

        public void Dispose()
        {
        }
    }

    private sealed class CompletedUnitOfWork : IUnitOfWork
    {
        public Task<bool> Commit()
        {
            return Task.FromResult(true);
        }
    }
}
