using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace AUT2Services.Infra.Security.Accounts.Yo;

public class YoCommandHandler(
    ITokenService tokenService,
    ICsrfService csrfService,
    ICurrentUserService currentUserService) : CommandHandler,
    IRequestHandler<YoCommand, CommandResponse>
{
    private readonly ITokenService tokenService = tokenService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly ICurrentUserService currentUserService = currentUserService;

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

        var expClaim = claimsPrincipal!.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        var expiresAtUtc = long.TryParse(expClaim, out var unixSeconds)
            ? DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime
            : DateTime.UtcNow;

        command.Request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var existingToken);
        csrfService.EnsureTokenCookie(command.Response, existingToken);

        CommandResponse.Data = JsonConvert.SerializeObject(new ProfileLogin(
            AccessTokenExpiracion: expiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            User: await tokenService.CreateUserDtoAsync(usuario, Guid.Parse(currentUser.EntidadIdSeleccionada!)),
            ProcesosActivos: await currentUserService.GetProcesosActivosAsync(cancellationToken) ?? null,
            CodigoOrganizacionSeleccionada: currentUser.OrganizacionSeleccionada,
            IdEntidadSeleccionada: currentUser.EntidadIdSeleccionada,
            SeleccionOrganizacionRequerida: currentUser.SeleccionOrganizacionrequerida
        ));
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
