using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AUT2Services.Infra.Security.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    UserManager<Usuario> userManager,
    ITokenService tokenService) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? SessionId => Principal?.FindFirstValue(JwtRegisteredClaimNames.Sid);

    public async Task<IList<ProcesoActivo>?> GetProcesosActivosAsync(CancellationToken cancellationToken = default)
    {
        IList<ProcesoActivo>? result = null;

        var principal = Principal;
        if (principal is null || !IsAuthenticated)
        {
            return null;
        }

        var procesos = Principal?.FindAll(EnumBusinessClaimTypes.PROCESO) ?? null;
        if (procesos is null)
        {
            return null;
        }

        await Task.Run(() =>
         {
             result = [];
             foreach (var proceso in procesos)
             {
                 result.Add(new ProcesoActivo(
                            Codigo: proceso.Value,
                            NombreProceso: Principal?.FindFirstValue($"{proceso.Value}{EnumPartialBusinessClaimTypes._NOMBRE}"),
                            Roles: Principal?.FindAll($"{proceso.Value}{EnumPartialBusinessClaimTypes._ROL}")
                                .Select(p => p.Value)
                                .ToList(),
                            NivelDeProceso: Principal?.FindFirstValue($"{proceso.Value}{EnumPartialBusinessClaimTypes._NIVEL_DE_PROCESO}"),
                            Url: Principal?.FindFirstValue($"{proceso.Value}{EnumPartialBusinessClaimTypes._URL}"),
                            Token: Principal?.FindFirstValue($"{proceso.Value}{EnumPartialBusinessClaimTypes._TOKEN}"),
                            ComoDesplegarUrlDeProceso: Principal?.FindFirstValue($"{proceso.Value}{EnumPartialBusinessClaimTypes._COMO_DESPLEGAR_URL}")
                     ));
             }
         }, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }

    public async Task<Usuario?> GetUserAsync(CancellationToken cancellationToken = default)
    {
        var principal = Principal;
        if (principal is null || !IsAuthenticated)
        {
            return null;
        }

        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();
        return user;
    }

    public ClaimsPrincipal? GetClaimsPrincipal(CancellationToken cancellationToken = default)
    {
        return Principal;
    }

    public async Task<CurrentUserResponse> GetCurrentUserResponseAsync(CancellationToken cancellationToken = default)
    {
        var usuario = await GetUserAsync(cancellationToken);
        if (usuario is null)
        {
            return new CurrentUserResponse(false, null, null, null, [], [], null, null, null, true);
        }

        var selectedOrganization = Principal?.FindFirstValue(EnumBusinessClaimTypes.CODIGO_ORGANIZACION) ?? null;
        var entidadSeleccionada = Principal?.FindFirstValue(EnumBusinessClaimTypes.ID_ENTIDAD) ?? null;
        var procesosActivos = await GetProcesosActivosAsync(cancellationToken) ?? null;
        var expClaim = Principal?.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        var expiresAtUtc = long.TryParse(expClaim, out var unixSeconds)
            ? DateTimeOffset.FromUnixTimeSeconds(unixSeconds).UtcDateTime
            : (DateTime?)null;
        var organizaiconesPorUsuario = await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id);

        return new CurrentUserResponse(
            true,
            usuario.Id,
            usuario.Email,
            usuario.NombreADesplegar,
            organizaiconesPorUsuario,
            procesosActivos,
            expiresAtUtc,
            selectedOrganization,
            entidadSeleccionada,
            string.IsNullOrWhiteSpace(selectedOrganization)
        );
    }
}
