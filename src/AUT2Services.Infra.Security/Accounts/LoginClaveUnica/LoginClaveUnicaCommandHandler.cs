using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Accounts.LoginClaveUnica;

public class LoginClaveUnicaCommandHandler(
    UserManager<Usuario> userManager,
    AUT2ServicesContext aUT2ServicesContext,
    ITokenService tokenService,
    IAuthCookieService authCookieService,
    ICsrfService csrfService,
    IEntidadRepository entidadRepository,
    IMediatorHandler mediator,
    IClaveUnicaClient claveUnicaClient,
    ISecurityTraceabilityService securityTraceabilityService,
    ILogger<LoginClaveUnicaCommandHandler> logger) : CommandHandler,
    IRequestHandler<LoginClaveUnicaCommand, CommandResponse>
{
    private const string LoginProvider = "CLAVEUNICA";
    private const string ProviderDisplayName = "Clave Única";

    private readonly UserManager<Usuario> userManager = userManager;
    private readonly AUT2ServicesContext aUT2ServicesContext = aUT2ServicesContext;
    private readonly ITokenService tokenService = tokenService;
    private readonly IAuthCookieService authCookieService = authCookieService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly IEntidadRepository entidadRepository = entidadRepository;
    private readonly IMediatorHandler mediator = mediator;
    private readonly IClaveUnicaClient claveUnicaClient = claveUnicaClient;
    private readonly ISecurityTraceabilityService securityTraceabilityService = securityTraceabilityService;
    private readonly ILogger<LoginClaveUnicaCommandHandler> logger = logger;

    public async Task<CommandResponse> Handle(LoginClaveUnicaCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!csrfService.IsRequestValid(command.Request))
        {
            AddError("Invalid CSRF token.");
            return CommandResponse;
        }

        if (!claveUnicaClient.IsConfigured)
        {
            AddError("La configuracion de Clave Unica no esta completa en el backend.");
            return CommandResponse;
        }

        ClaveUnicaUserInfoResponse? userInfo;

        try
        {
            var accessToken = await claveUnicaClient.ExchangeCodeAsync(command.Code!, cancellationToken);
            userInfo = await claveUnicaClient.GetUserInfoAsync(accessToken, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No fue posible completar la comunicacion con Clave Unica.");
            AddError("No fue posible autenticar con Clave Unica.");
            return CommandResponse;
        }

        var run = NormalizeRun(userInfo);
        if (run is null)
        {
            AddError("Clave Unica no devolvio un RUN valido para el ciudadano autenticado.");
            return CommandResponse;
        }

        try
        {
            var usuario = await FindOrCreateUserAsync(command, userInfo!, run, cancellationToken);
            var entidad = await EnsureBaseEntityAsync(usuario, userInfo!, run, cancellationToken);
            var profileLogin = await IssueSessionAsync(command.Response, usuario, entidad, cancellationToken);

            csrfService.EnsureTokenCookie(command.Response);
            CommandResponse.Data = profileLogin;
            CommandResponse.Result = true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible completar el login de Clave Unica.");
            AddError("No fue posible completar el login con Clave Unica.");
        }

        return CommandResponse;
    }

    private async Task<Usuario> FindOrCreateUserAsync(
        LoginClaveUnicaCommand command,
        ClaveUnicaUserInfoResponse userInfo,
        ClaveUnicaRun run,
        CancellationToken cancellationToken)
    {
        var providerKey = run.Formatted;
        var placeholderEmail = BuildPlaceholderEmail(run);

        var usuario = await userManager.FindByLoginAsync(LoginProvider, providerKey);
        usuario ??= await FindUserByRunAsync(run, cancellationToken);
        usuario ??= await userManager.FindByEmailAsync(placeholderEmail);

        using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
        var wasCreated = false;
        try
        {
            if (usuario is null)
            {
                usuario = await CreateClaveUnicaUserAsync(userInfo, run);
                wasCreated = true;
            }

            await EnsureExternalLoginAsync(usuario, providerKey);
            if (wasCreated)
            {
                securityTraceabilityService.TrackCreate(
                    command,
                    usuario.Id,
                    SecurityTraceabilityEventTypes.UsuarioRegistrado,
                    UsuarioTraceabilityState.FromUser(usuario) with
                    {
                        ActionContext = "CLAVE_UNICA",
                        RequestPath = command.Request.Path
                    });

                if (!await aUT2ServicesContext.Commit())
                {
                    throw new InvalidOperationException("No fue posible persistir la trazabilidad del registro mediante Clave Unica.");
                }
            }

            await aUT2ServicesContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch
        {
            await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            throw;
        }

        return usuario;
    }

    private async Task<Usuario> CreateClaveUnicaUserAsync(ClaveUnicaUserInfoResponse userInfo, ClaveUnicaRun run)
    {
        var (primerNombre, segundoNombre, primerApellido, segundoApellido, nombreADesplegar) = ExtractNames(userInfo);
        var placeholderEmail = BuildPlaceholderEmail(run);

        var usuario = new Usuario
        {
            UserName = placeholderEmail,
            NormalizedUserName = placeholderEmail.ToUpperInvariant(),
            Email = placeholderEmail,
            NormalizedEmail = placeholderEmail.ToUpperInvariant(),
            EmailConfirmed = true,
            PhoneNumber = string.Empty,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            AccessFailedCount = 0,
            IdPersona = run.Formatted,
            NombreADesplegar = nombreADesplegar,
            Descripcion = "Usuario autenticado mediante Clave Unica.",
            TipoDeUsuario = EnumTipoDeUsuario.NACIONAL,
            Activo = true,
            UsuarioBase = false,
            RequiereValidacionEnrrolamiento = false,
            EstadoDeUsuario = EnumEstadoDeUsuario.REGISTRADO,
            InformacionAdicional = JsonSerializer.Serialize(new InformacionAdicionalModel
            {
                Nacionalidad = EnumNacionalidad.Chileno_a,
                DocumentoDeIdentidad = EnumDocumentoDeIdentidad.RUN,
                NumeroDeDocumento = run.Number,
                CodigoValidadorDocumento = run.Dv,
                PrimerNombre = primerNombre,
                SegundoNombre = segundoNombre,
                PrimerApellido = primerApellido,
                SegundoApellido = segundoApellido,
                TerminosYCondiciones = true
            })
        };

        var creationResult = await userManager.CreateAsync(usuario);
        if (!creationResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join(" ", creationResult.Errors.Select(x => $"{x.Code} - {x.Description}")));
        }

        return await userManager.FindByIdAsync(usuario.Id)
            ?? throw new InvalidOperationException("No fue posible recuperar el usuario Clave Unica recien creado.");
    }

    private async Task EnsureExternalLoginAsync(Usuario usuario, string providerKey)
    {
        var logins = await userManager.GetLoginsAsync(usuario);
        var claveUnicaLogins = logins
            .Where(login => login.LoginProvider.Equals(LoginProvider, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (claveUnicaLogins.Any(login => login.ProviderKey.Equals(providerKey, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        foreach (var login in claveUnicaLogins)
        {
            var removeResult = await userManager.RemoveLoginAsync(usuario, login.LoginProvider, login.ProviderKey);
            if (!removeResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(" ", removeResult.Errors.Select(x => $"{x.Code} - {x.Description}")));
            }
        }

        var addResult = await userManager.AddLoginAsync(usuario, new UserLoginInfo(LoginProvider, providerKey, ProviderDisplayName));
        if (!addResult.Succeeded)
        {
            throw new InvalidOperationException(string.Join(" ", addResult.Errors.Select(x => $"{x.Code} - {x.Description}")));
        }
    }

    private async Task<Usuario?> FindUserByRunAsync(ClaveUnicaRun run, CancellationToken cancellationToken)
    {
        var numeroToken = $"\"NumeroDeDocumento\":\"{run.Number}\"";
        var dvToken = $"\"CodigoValidadorDocumento\":\"{run.Dv}\"";
        var tipoToken = $"\"DocumentoDeIdentidad\":\"{EnumDocumentoDeIdentidad.RUN}\"";

        var candidatos = await userManager.Users
            .Where(user => user.InformacionAdicional != null
                && user.InformacionAdicional.Contains(numeroToken)
                && user.InformacionAdicional.Contains(dvToken)
                && user.InformacionAdicional.Contains(tipoToken))
            .ToListAsync(cancellationToken);

        foreach (var candidato in candidatos)
        {
            if (string.IsNullOrWhiteSpace(candidato.InformacionAdicional))
            {
                continue;
            }

            var informacion = JsonSerializer.Deserialize<InformacionAdicionalModel>(candidato.InformacionAdicional);
            if (informacion is null)
            {
                continue;
            }

            if (!EnumDocumentoDeIdentidad.RUN.Equals(informacion.DocumentoDeIdentidad, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!run.Number.Equals(NormalizeDigits(informacion.NumeroDeDocumento), StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!run.Dv.Equals(NormalizeDv(informacion.CodigoValidadorDocumento), StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            return candidato;
        }

        return null;
    }

    private async Task<Entidad> EnsureBaseEntityAsync(
        Usuario usuario,
        ClaveUnicaUserInfoResponse userInfo,
        ClaveUnicaRun run,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(usuario.Id);
        var personas = (await entidadRepository.BuscarPor_Id_Usuario(userId))
            .Where(entidad => EnumTipoDeEntidad.PERSONA.Equals(entidad.TipoDeEntidad, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (personas.Count == 0)
        {
            var (_, _, _, _, nombreADesplegar) = ExtractNames(userInfo);
            var baseEntityCommand = new BaseEntityCommand
            {
                CodigoOrganizacion = run.Number,
                NombreOrganizacion = nombreADesplegar,
                Id_Usuario = userId,
                TipoDeEntidad = EnumTipoDeEntidad.PERSONA,
                CorreoElectronico = string.Empty,
                PermitirCorreoElectronicoVacio = true
            };

            var result = await mediator.SendCommand(baseEntityCommand, cancellationToken);
            if (!result.Result)
            {
                throw new InvalidOperationException(string.Join(" ", result.ValidationResult.Errors.Select(x => x.ErrorMessage)));
            }

            personas = (await entidadRepository.BuscarPor_Id_Usuario(userId))
                .Where(entidad => EnumTipoDeEntidad.PERSONA.Equals(entidad.TipoDeEntidad, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (personas.Count == 1 && personas[0].Principal)
        {
            return personas[0];
        }

        logger.LogWarning(
            "Se bloqueo el login de Clave Unica porque la entidad PERSONA no cumple la invariancia requerida. PersonaCount: {PersonaCount}; PrincipalCount: {PrincipalCount}.",
            personas.Count,
            personas.Count(persona => persona.Principal));
        throw new InvalidOperationException("La entidad PERSONA del usuario no cumple la invariancia requerida.");
    }

    private async Task<ProfileLogin> IssueSessionAsync(
        HttpResponse response,
        Usuario usuario,
        Entidad entidad,
        CancellationToken cancellationToken)
    {
        var sessionId = Guid.NewGuid().ToString("N");
        var accessTokenResult = await tokenService.GenerateAccessTokenAsync(usuario, sessionId);
        var refreshToken = tokenService.CreateRefreshToken(sessionId);
        refreshToken.RefreshToken.UserId = usuario.Id;
        refreshToken.RefreshToken.Id_Entidad = entidad.Id;

        aUT2ServicesContext.RefreshTokens.Add(refreshToken.RefreshToken);
        await aUT2ServicesContext.SaveChangesAsync(cancellationToken);

        authCookieService.AppendAuthCookies(
            response,
            accessTokenResult,
            refreshToken.RefreshToken.ExpiresAtUtc,
            refreshToken.PlainTextToken);

        return new ProfileLogin(
            AccessTokenExpiracion: accessTokenResult.ExpiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            UnidadesOrganizacionalesPorUsuario: null,
            User: await tokenService.CreateUserDtoAsync(usuario, entidad.Id),
            ProcesosActivos: null,
            CodigoOrganizacionSeleccionada: null,
            NombreOrganizacionSeleccionada: null,
            CodigoUnidadOrganizacionalSeleccionada: null,
            NombreUnidadOrganizacionalSeleccionada: null,
            IdEntidadSeleccionada: null,
            EntidadRolSeleccionado: null,
            SeleccionOrganizacionRequerida: true);
    }

    private static ClaveUnicaRun? NormalizeRun(ClaveUnicaUserInfoResponse? userInfo)
    {
        if (userInfo?.RolUnico is null)
        {
            return null;
        }

        var tipo = userInfo.RolUnico.Tipo?.Trim();
        if (!EnumDocumentoDeIdentidad.RUN.Equals(tipo, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var numero = NormalizeDigits(userInfo.RolUnico.Numero?.ToString());
        var dv = NormalizeDv(userInfo.RolUnico.Dv);

        if (string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(dv))
        {
            return null;
        }

        return new ClaveUnicaRun(numero, dv);
    }

    private static string NormalizeDigits(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return new string(value.Where(char.IsDigit).ToArray());
    }

    private static string NormalizeDv(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().ToUpperInvariant();
    }

    private static string BuildPlaceholderEmail(ClaveUnicaRun run)
    {
        return $"claveunica-{run.Number}-{run.Dv.ToLowerInvariant()}@anid.local";
    }

    private static (string PrimerNombre, string SegundoNombre, string PrimerApellido, string SegundoApellido, string NombreADesplegar)
        ExtractNames(ClaveUnicaUserInfoResponse userInfo)
    {
        var nombres = userInfo.Name?.Nombres?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            .ToArray() ?? [];
        var apellidos = userInfo.Name?.Apellidos?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            .ToArray() ?? [];

        var primerNombre = nombres.ElementAtOrDefault(0) ?? string.Empty;
        var segundoNombre = nombres.Length > 1 ? string.Join(" ", nombres.Skip(1)) : string.Empty;
        var primerApellido = apellidos.ElementAtOrDefault(0) ?? string.Empty;
        var segundoApellido = apellidos.Length > 1 ? string.Join(" ", apellidos.Skip(1)) : string.Empty;
        var nombreADesplegar = string.Join(" ", new[] { primerNombre, primerApellido }.Where(value => !string.IsNullOrWhiteSpace(value)));

        if (string.IsNullOrWhiteSpace(nombreADesplegar))
        {
            nombreADesplegar = "Usuario Clave Unica";
        }

        return (primerNombre, segundoNombre, primerApellido, segundoApellido, nombreADesplegar);
    }

    private sealed record ClaveUnicaRun(string Number, string Dv)
    {
        public string Formatted => $"{Number}-{Dv}";
    }
}
