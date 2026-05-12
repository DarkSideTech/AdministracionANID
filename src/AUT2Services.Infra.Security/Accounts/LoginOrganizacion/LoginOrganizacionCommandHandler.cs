using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommandHandler(
    ISecurityRepository securityRepository,
    ITokenService tokenService,
    IOrganizacionRepository organizacionRepository,
    IAuthCookieService authCookieService,
    ICsrfService csrfService,
    AUT2ServicesContext aUT2ServicesContext,
    ICurrentUserService currentUserService) : CommandHandler,
    IRequestHandler<LoginOrganizacionCommand, CommandResponse>
{
    private readonly ISecurityRepository securityRepository = securityRepository;
    private readonly ITokenService tokenService = tokenService;
    private readonly IOrganizacionRepository organizacionRepository = organizacionRepository;
    private readonly IAuthCookieService authCookieService = authCookieService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly AUT2ServicesContext aUT2ServicesContext = aUT2ServicesContext;
    private readonly ICurrentUserService currentUserService = currentUserService;

    public async Task<CommandResponse> Handle(LoginOrganizacionCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        AccessTokenResult? accessTokenResult = null;
        Entidad? entidad = null;
        Organizacion? organizacion = null;
        Usuario? usuario = null;
        Guid? idRolSeleccionado = null;

        try
        {
            if (!csrfService.IsRequestValid(command.Request))
            {
                AddError("Invalid CSRF token.");
                return CommandResponse;
            }

            var user = await currentUserService.GetUserAsync(command.Context.RequestAborted);
            if (user is null || string.IsNullOrWhiteSpace(currentUserService.SessionId))
            {
                authCookieService.ClearAuthCookies(command.Response);
                AddError("Usuario no autorizado.");
                return CommandResponse;
            }
            else
            {
                usuario = user;
            }

            if (string.IsNullOrWhiteSpace(command.Organizacion))
            {
                AddError("La organizacion no existe");
                return CommandResponse;
            }

            if (!string.IsNullOrEmpty(command.Organizacion))
            {
                organizacion = await organizacionRepository.BuscarPor_Codigo(command.Organizacion!);

                if (organizacion is null)
                {
                    AddError($"La organizacion no existe");
                    return CommandResponse;
                }

                entidad = await securityRepository.BuscarEntidadPrincipalPorUsuarioOrganizacion(Guid.Parse(user.Id), organizacion.Id);
            }

            if (entidad is null)
            {
                AddError("El usuario no cuenta con una entidad principal habilitada para la organizacion seleccionada.");
                return CommandResponse;
            }

            var selectedContextForToken = await currentUserService.GetSelectedSessionContextAsync(
                entidad.Id,
                cancellationToken: command.Context.RequestAborted);
            idRolSeleccionado = selectedContextForToken?.EntidadRolSeleccionado?.Id_Rol;
            if (!idRolSeleccionado.HasValue)
            {
                AddError("No fue posible determinar el rol activo de la organizacion seleccionada.");
                return CommandResponse;
            }

            await tokenService.RevokeSessionAsync(currentUserService.SessionId, EnumRefreshTokenRevocationReasons.SecondLogin);

            var sessionId = Guid.NewGuid().ToString("N");
            accessTokenResult = await tokenService.GenerateAccessTokenAsync(usuario, sessionId, entidad.Id, idRolSeleccionado.Value);
            var refreshToken = tokenService.CreateRefreshToken(
                sessionId,
                SelectedOrganizationSessionSerializer.Serialize(
                    selectedContextForToken?.CodigoOrganizacionSeleccionada ?? organizacion!.Codigo,
                    idRolSeleccionado.Value));
            refreshToken.RefreshToken.UserId = usuario.Id;
            refreshToken.RefreshToken.Id_Entidad = entidad.Id;

            aUT2ServicesContext.RefreshTokens.Add(refreshToken.RefreshToken);
            await aUT2ServicesContext.SaveChangesAsync(cancellationToken);

            authCookieService.AppendAuthCookies(
                command.Response,
                accessTokenResult,
                refreshToken.RefreshToken.ExpiresAtUtc,
                refreshToken.PlainTextToken
            );
            csrfService.EnsureTokenCookie(command.Response);
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
            return CommandResponse;
        }

        if (accessTokenResult is null || usuario is null || entidad is null || organizacion is null)
        {
            AddError("No fue posible completar el login para la organizacion seleccionada.");
            return CommandResponse;
        }

        var selectedContext = await currentUserService.GetSelectedSessionContextAsync(
            entidad.Id,
            cancellationToken: command.Context.RequestAborted);
        var idRolSeleccionadoContext = selectedContext?.EntidadRolSeleccionado?.Id_Rol;
        var procesosActivos = idRolSeleccionadoContext.HasValue
            ? await currentUserService.GetProcesosActivosPorEntidadAsync(
                entidad.Id,
                idRolSeleccionadoContext.Value,
                command.Context.RequestAborted)
            : null;

        CommandResponse.Data = new ProfileLogin(
            AccessTokenExpiracion: accessTokenResult.ExpiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            UnidadesOrganizacionalesPorUsuario: await currentUserService.GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(
                entidad.Id,
                Guid.Parse(usuario.Id),
                command.Context.RequestAborted),
            User: await tokenService.CreateUserDtoAsync(
                usuario,
                entidad.Id,
                selectedContext?.EntidadRolSeleccionado),
            ProcesosActivos: procesosActivos,
            CodigoOrganizacionSeleccionada: selectedContext?.CodigoOrganizacionSeleccionada ?? organizacion.Codigo,
            NombreOrganizacionSeleccionada: selectedContext?.NombreOrganizacionSeleccionada ?? organizacion.Nombre,
            CodigoUnidadOrganizacionalSeleccionada: selectedContext?.CodigoUnidadOrganizacionalSeleccionada,
            NombreUnidadOrganizacionalSeleccionada: selectedContext?.NombreUnidadOrganizacionalSeleccionada,
            IdEntidadSeleccionada: entidad.Id.ToString(),
            EntidadRolSeleccionado: selectedContext?.EntidadRolSeleccionado,
            SeleccionOrganizacionRequerida: false
        );
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
