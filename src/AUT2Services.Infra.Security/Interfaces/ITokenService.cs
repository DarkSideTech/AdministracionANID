using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Records;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ITokenService
{
    Task<AccessTokenResult> GenerateAccessTokenAsync(Usuario user, string sessionId, Guid? idEntidad = null, Guid? idRol = null);
    RefreshTokenIssuanceResult CreateRefreshToken(string sessionId, string? selectedOrganization = null);
    Task<IList<OrganizacionPorUsuario>> BuscarOrganizacionesPorIdUsuario(string idUsuario);
    string HashRefreshToken(string refreshToken);
    Task<UserDto> CreateUserDtoAsync(
        Usuario user,
        Guid? id_Entidad = null,
        AUT2Services.Infra.Security.Models.EntidadRolSeleccionado? entidadRolSeleccionado = null);
    Task RevokeSessionAsync(string sessionId, string reason);
}
