using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Records;
using System.Security.Claims;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? SessionId { get; }
    Task<Usuario?> GetUserAsync(CancellationToken cancellationToken = default);
    Task<IList<ProcesoActivo>?> GetProcesosActivosAsync(CancellationToken cancellationToken = default);
    Task<IList<ProcesoActivo>?> GetProcesosActivosPorEntidadAsync(Guid idEntidad, Guid idRol, CancellationToken cancellationToken = default);
    Task<IList<UnidadOrganizacionalEntidadRolPorUsuario>> GetUnidadesOrganizacionalesEntidadRolPorUsuarioAsync(Guid idEntidad, Guid idUsuario, CancellationToken cancellationToken = default);
    Task<SelectedSessionContext?> GetSelectedSessionContextAsync(Guid idEntidad, Guid? idRol = null, CancellationToken cancellationToken = default);
    IList<ProcesoActivo>? GetProcesosActivos(IEnumerable<Claim>? claims);
    ClaimsPrincipal? GetClaimsPrincipal(CancellationToken cancellationToken = default);
    Task<CurrentUserResponse> GetCurrentUserResponseAsync(CancellationToken cancellationToken = default);
}
