using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace AUT2Services.Infra.Security.Accounts.Yo;

public class YoCommandHandler(
    ITokenService tokenService,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    IClock clock) : CommandHandler,
    IRequestHandler<YoCommand, CommandResponse>
{
    private readonly ITokenService tokenService = tokenService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly ICurrentUserService currentUserService = currentUserService;
    private readonly IClock clock = clock;

    public async Task<CommandResponse> Handle(YoCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var usuario = await currentUserService.GetUserAsync(command.Context.RequestAborted);
        if (usuario is null)
        {
            AddError("Usuario no autorizado.");
            return CommandResponse;
        }

        var currentUser = await currentUserService.GetCurrentUserResponseAsync(command.Context.RequestAborted);
        var claimsPrincipal = currentUserService.GetClaimsPrincipal(command.Context.RequestAborted);
        var selectedEntityRole = currentUser.EntidadRolSeleccionado;

        var expClaim = claimsPrincipal!.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        var expiresAtUtc = long.TryParse(expClaim, out var unixSeconds)
            ? DateTimeOffset.FromUnixTimeSeconds(unixSeconds)
            : clock.UtcNow;

        command.Request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var existingToken);
        csrfService.EnsureTokenCookie(command.Response, existingToken);

        var userDto = Guid.TryParse(currentUser.EntidadIdSeleccionada, out var idEntidadSeleccionada)
            ? await tokenService.CreateUserDtoAsync(usuario, idEntidadSeleccionada, selectedEntityRole)
            : await tokenService.CreateUserDtoAsync(usuario);

        CommandResponse.Data = new ProfileLogin(
            AccessTokenExpiracion: expiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            UnidadesOrganizacionalesPorUsuario: currentUser.UnidadesOrganizacionalesPorUsuario,
            User: userDto,
            ProcesosActivos: currentUser.ProcesosActivos,
            CodigoOrganizacionSeleccionada: currentUser.OrganizacionSeleccionada,
            NombreOrganizacionSeleccionada: currentUser.NombreOrganizacionSeleccionada,
            CodigoUnidadOrganizacionalSeleccionada: currentUser.CodigoUnidadOrganizacionalSeleccionada,
            NombreUnidadOrganizacionalSeleccionada: currentUser.NombreUnidadOrganizacionalSeleccionada,
            IdEntidadSeleccionada: currentUser.EntidadIdSeleccionada,
            EntidadRolSeleccionado: selectedEntityRole,
            SeleccionOrganizacionRequerida: currentUser.SeleccionOrganizacionrequerida
        );
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
