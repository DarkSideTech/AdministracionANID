using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using System.Security.Claims;

namespace AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;

public class CambioUnidadOrganizacionalEntidadRolCommandHandler(
    ITokenService tokenService,
    ICurrentUserService currentUserService,
    IAuthCookieService authCookieService,
    ICsrfService csrfService,
    AUT2ServicesContext aUT2ServicesContext) : CommandHandler,
    IRequestHandler<CambioUnidadOrganizacionalEntidadRolCommand, CommandResponse>
{
    private const string InvalidSelectionMessage = "La unidad Organizacion y Rol seleccionado no permite cambiar la configuracion, contactarse con el Administrador";

    private readonly ITokenService tokenService = tokenService;
    private readonly ICurrentUserService currentUserService = currentUserService;
    private readonly IAuthCookieService authCookieService = authCookieService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly AUT2ServicesContext aUT2ServicesContext = aUT2ServicesContext;

    public async Task<CommandResponse> Handle(CambioUnidadOrganizacionalEntidadRolCommand command, CancellationToken cancellationToken)
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

        var usuario = await currentUserService.GetUserAsync(command.Context.RequestAborted);
        var principal = currentUserService.GetClaimsPrincipal(command.Context.RequestAborted);
        if (usuario is null || principal is null || string.IsNullOrWhiteSpace(currentUserService.SessionId))
        {
            authCookieService.ClearAuthCookies(command.Response);
            AddError("Usuario no autorizado.");
            return CommandResponse;
        }

        if (!Guid.TryParse(command.Id_Entidad, out var idEntidadSeleccionada)
            || !Guid.TryParse(command.Id_Rol, out var idRolSeleccionado))
        {
            AddError(InvalidSelectionMessage);
            return CommandResponse;
        }

        var entidadActualClaim = principal.FindFirstValue(EnumBusinessClaimTypes.ID_ENTIDAD);
        if (!Guid.TryParse(entidadActualClaim, out var idEntidadActual))
        {
            AddError(InvalidSelectionMessage);
            return CommandResponse;
        }

        var unidadesDisponibles = await currentUserService.GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(
            idEntidadActual,
            Guid.Parse(usuario.Id),
            command.Context.RequestAborted);
        var seleccionValida = unidadesDisponibles.Any(item =>
            string.Equals(item.Id_Entidad, idEntidadSeleccionada.ToString(), StringComparison.OrdinalIgnoreCase)
            && string.Equals(item.Id_Rol, idRolSeleccionado.ToString(), StringComparison.OrdinalIgnoreCase));

        if (!seleccionValida)
        {
            AddError(InvalidSelectionMessage);
            return CommandResponse;
        }

        var selectedContext = await currentUserService.GetSelectedSessionContextAsync(
            idEntidadSeleccionada,
            idRolSeleccionado,
            command.Context.RequestAborted);
        if (selectedContext is null)
        {
            AddError(InvalidSelectionMessage);
            return CommandResponse;
        }

        await tokenService.RevokeSessionAsync(currentUserService.SessionId, EnumRefreshTokenRevocationReasons.SecondLogin);

        var sessionId = Guid.NewGuid().ToString("N");
        var accessTokenResult = await tokenService.GenerateAccessTokenAsync(
            usuario,
            sessionId,
            idEntidadSeleccionada,
            idRolSeleccionado);
        var refreshToken = tokenService.CreateRefreshToken(
            sessionId,
            SelectedOrganizationSessionSerializer.Serialize(
                selectedContext.CodigoOrganizacionSeleccionada,
                idRolSeleccionado));
        refreshToken.RefreshToken.UserId = usuario.Id;
        refreshToken.RefreshToken.Id_Entidad = idEntidadSeleccionada;

        aUT2ServicesContext.RefreshTokens.Add(refreshToken.RefreshToken);
        await aUT2ServicesContext.SaveChangesAsync(cancellationToken);

        authCookieService.AppendAuthCookies(
            command.Response,
            accessTokenResult,
            refreshToken.RefreshToken.ExpiresAtUtc,
            refreshToken.PlainTextToken);
        csrfService.EnsureTokenCookie(command.Response);

        CommandResponse.Data = new ProfileLogin(
            AccessTokenExpiracion: accessTokenResult.ExpiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            UnidadesOrganizacionalesPorUsuario: await currentUserService.GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(
                idEntidadSeleccionada,
                Guid.Parse(usuario.Id),
                command.Context.RequestAborted),
            User: await tokenService.CreateUserDtoAsync(
                usuario,
                idEntidadSeleccionada,
                selectedContext.EntidadRolSeleccionado),
            ProcesosActivos: await currentUserService.GetProcesosActivosPorEntidadAsync(
                idEntidadSeleccionada,
                idRolSeleccionado,
                command.Context.RequestAborted),
            CodigoOrganizacionSeleccionada: selectedContext.CodigoOrganizacionSeleccionada,
            NombreOrganizacionSeleccionada: selectedContext.NombreOrganizacionSeleccionada,
            CodigoUnidadOrganizacionalSeleccionada: selectedContext.CodigoUnidadOrganizacionalSeleccionada,
            NombreUnidadOrganizacionalSeleccionada: selectedContext.NombreUnidadOrganizacionalSeleccionada,
            IdEntidadSeleccionada: idEntidadSeleccionada.ToString(),
            EntidadRolSeleccionado: selectedContext.EntidadRolSeleccionado,
            SeleccionOrganizacionRequerida: false
        );
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
