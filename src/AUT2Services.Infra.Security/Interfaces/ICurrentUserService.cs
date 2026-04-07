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
    ClaimsPrincipal? GetClaimsPrincipal(CancellationToken cancellationToken = default);
    Task<CurrentUserResponse> GetCurrentUserResponseAsync(CancellationToken cancellationToken = default);
}
