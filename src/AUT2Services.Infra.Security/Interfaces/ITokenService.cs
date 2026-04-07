using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Records;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ITokenService
{
    Task<AccessTokenResult> GenerateAccessTokenAsync(Usuario user, string sessionId, Guid? idEntidad = null);
    RefreshTokenIssuanceResult CreateRefreshToken(string sessionId, string? selectedOrganization = null);
    Task<IList<OrganizacionesPorUsuario>> BuscarOrganizacionesPorIdUsuario(string idUsuario);
    string HashRefreshToken(string refreshToken);
    Task<UserDto> CreateUserDtoAsync(Usuario user, Guid id_Entidad);
    Task RevokeSessionAsync(string sessionId, string reason);
}