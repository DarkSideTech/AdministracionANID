using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
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
    ITokenService tokenService,
    ISecurityRepository securityRepository,
    IEntidadRepository entidadRepository,
    IUnidadOrganizacionalRepository unidadOrganizacionalRepository,
    IOrganizacionRepository organizacionRepository) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated ?? false;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? SessionId => Principal?.FindFirstValue(JwtRegisteredClaimNames.Sid);

    public async Task<IList<ProcesoActivo>?> GetProcesosActivosAsync(CancellationToken cancellationToken = default)
    {
        var principal = Principal;
        if (principal is null || !IsAuthenticated)
        {
            return null;
        }

        cancellationToken.ThrowIfCancellationRequested();

        var selectedOrganization = principal.FindFirstValue(EnumBusinessClaimTypes.CODIGO_ORGANIZACION);
        var entidadSeleccionada = principal.FindFirstValue(EnumBusinessClaimTypes.ID_ENTIDAD);
        var rolSeleccionado = principal.FindFirstValue(EnumBusinessClaimTypes.ID_ROL);
        if (!string.IsNullOrWhiteSpace(selectedOrganization)
            && Guid.TryParse(entidadSeleccionada, out var idEntidad))
        {
            var idRolSeleccionado = TryParseGuid(rolSeleccionado);
            if (!idRolSeleccionado.HasValue)
            {
                var selectedContext = await GetSelectedSessionContextAsync(idEntidad, cancellationToken: cancellationToken);
                idRolSeleccionado = selectedContext?.EntidadRolSeleccionado?.Id_Rol;
            }

            if (idRolSeleccionado.HasValue)
            {
                return await GetProcesosActivosPorEntidadAsync(idEntidad, idRolSeleccionado.Value, cancellationToken);
            }

            return null;
        }

        return await Task.FromResult(GetProcesosActivos(principal.Claims));
    }

    public async Task<IList<ProcesoActivo>?> GetProcesosActivosPorEntidadAsync(Guid idEntidad, Guid idRol, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var policies = await securityRepository.BuscarTodasLasPolicies(idEntidad, idRol);
        return GetProcesosActivos(
            policies.Select(policy => new Claim(policy.ClaimType, policy.ClaimValue)));
    }

    public async Task<IList<UnidadOrganizacionalEntidadRolPorUsuario>> GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(Guid idEntidad, Guid idUsuario, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entidad = await entidadRepository.BuscarPor_Id(idEntidad);
        if (entidad is null)
        {
            return [];
        }

        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional);
        if (unidadOrganizacional is null)
        {
            return [];
        }

        return await securityRepository.BuscarUnidadesOrganizacionalesEntidadRolPor_Id_Organizacion(
            unidadOrganizacional.Id_Organizacion,
            idUsuario);
    }

    public async Task<SelectedSessionContext?> GetSelectedSessionContextAsync(Guid idEntidad, Guid? idRol = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entidad = await entidadRepository.BuscarPor_Id(idEntidad);
        if (entidad is null)
        {
            return null;
        }

        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional);
        if (unidadOrganizacional is null)
        {
            return null;
        }

        var organizacion = await organizacionRepository.BuscarPor_Id(unidadOrganizacional.Id_Organizacion);
        if (organizacion is null)
        {
            return null;
        }

        var entidadRolSeleccionado = await securityRepository.BuscarEntidadRolSeleccionadoPor_Id_Entidad(idEntidad, idRol);

        return new SelectedSessionContext(
            CodigoOrganizacionSeleccionada: organizacion.Codigo,
            NombreOrganizacionSeleccionada: organizacion.Nombre,
            CodigoUnidadOrganizacionalSeleccionada: unidadOrganizacional.Codigo,
            NombreUnidadOrganizacionalSeleccionada: unidadOrganizacional.Nombre,
            EntidadRolSeleccionado: entidadRolSeleccionado
        );
    }

    public IList<ProcesoActivo>? GetProcesosActivos(IEnumerable<Claim>? claims)
    {
        if (claims is null)
        {
            return null;
        }

        var claimsList = claims.ToList();
        var procesos = claimsList
            .Where(claim => claim.Type == EnumBusinessClaimTypes.PROCESO)
            .Select(claim => claim.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (procesos.Count == 0)
        {
            return null;
        }

        IList<ProcesoActivo> result = [];

        foreach (var proceso in procesos)
        {
            result.Add(new ProcesoActivo(
                IdProceso: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._ID_PROCESO}"),
                IdMacroProceso: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._ID_MACRO_PROCESO}"),
                Codigo: proceso,
                NombreProceso: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._NOMBRE}"),
                Roles: claimsList
                    .Where(claim => claim.Type == $"{proceso}{EnumPartialBusinessClaimTypes._ROL}")
                    .Select(claim => claim.Value)
                    .ToList(),
                NivelDeProceso: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._NIVEL_DE_PROCESO}"),
                Url: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._URL}"),
                Token: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._TOKEN}"),
                ComoDesplegarUrlDeProceso: claimsList.FindFirstValue($"{proceso}{EnumPartialBusinessClaimTypes._COMO_DESPLEGAR_URL}")
            ));
        }

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
            return new CurrentUserResponse(false, null, null, null, null, [], [], [], null, null, null, null, null, null, null, true);
        }

        var entidadSeleccionada = Principal?.FindFirstValue(EnumBusinessClaimTypes.ID_ENTIDAD) ?? null;
        var organizacionSeleccionada = Principal?.FindFirstValue(EnumBusinessClaimTypes.CODIGO_ORGANIZACION) ?? null;
        var rolSeleccionado = Principal?.FindFirstValue(EnumBusinessClaimTypes.ID_ROL) ?? null;
        var expClaim = Principal?.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        var expiresAtUtc = long.TryParse(expClaim, out var unixSeconds)
            ? DateTimeOffset.FromUnixTimeSeconds(unixSeconds)
            : (DateTimeOffset?)null;
        var organizaiconesPorUsuario = await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id);
        Guid? idEntidadSeleccionada = Guid.TryParse(entidadSeleccionada, out var parsedIdEntidad)
            ? parsedIdEntidad
            : null;
        var hasSelectedOrganization = !string.IsNullOrWhiteSpace(organizacionSeleccionada)
            && idEntidadSeleccionada.HasValue;
        var idRolSeleccionadoDesdeClaim = TryParseGuid(rolSeleccionado);
        var selectedContext = hasSelectedOrganization
            ? await GetSelectedSessionContextAsync(
                idEntidadSeleccionada!.Value,
                idRolSeleccionadoDesdeClaim,
                cancellationToken)
            : null;
        var idRolSeleccionado = selectedContext?.EntidadRolSeleccionado?.Id_Rol;
        var procesosActivos = hasSelectedOrganization && idRolSeleccionado.HasValue
            ? await GetProcesosActivosPorEntidadAsync(idEntidadSeleccionada!.Value, idRolSeleccionado.Value, cancellationToken)
            : null;
        var unidadesOrganizacionalesPorUsuario = hasSelectedOrganization
            ? await GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(idEntidadSeleccionada!.Value, Guid.Parse(usuario.Id), cancellationToken)
            : [];
        var selectedOrganization = hasSelectedOrganization
            ? selectedContext?.CodigoOrganizacionSeleccionada ?? organizacionSeleccionada
            : null;
        var userDto = await tokenService.CreateUserDtoAsync(
            usuario,
            idEntidadSeleccionada,
            selectedContext?.EntidadRolSeleccionado);

        return new CurrentUserResponse(
            true,
            usuario.Id,
            usuario.Email,
            usuario.NombreADesplegar,
            userDto,
            organizaiconesPorUsuario,
            unidadesOrganizacionalesPorUsuario,
            procesosActivos,
            expiresAtUtc,
            selectedOrganization,
            selectedContext?.NombreOrganizacionSeleccionada,
            selectedContext?.CodigoUnidadOrganizacionalSeleccionada,
            selectedContext?.NombreUnidadOrganizacionalSeleccionada,
            hasSelectedOrganization ? entidadSeleccionada : null,
            selectedContext?.EntidadRolSeleccionado,
            string.IsNullOrWhiteSpace(selectedOrganization)
        );
    }

    private static Guid? TryParseGuid(string? value)
    {
        return Guid.TryParse(value, out var parsedValue) ? parsedValue : null;
    }
}

file static class ClaimCollectionExtensions
{
    public static string? FindFirstValue(this IEnumerable<Claim> claims, string claimType)
    {
        return claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }
}
