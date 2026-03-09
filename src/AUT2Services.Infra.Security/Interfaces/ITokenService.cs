using AUT2Services.Domain.Security.Entities;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ITokenService
{
    Task<(string jwtToken, DateTime expiresAtUtc)> GenerateJwtTokenLoginOrganizacion(Usuario user, Guid id_Entidad);
    (string jwtToken, DateTime expiresAtUtc) GenerateJwtTokenLogin(Usuario user);
    string GenerateRefreshToken();
    void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
    void DeleteAuthCookie(string cookieName);
    string GetAuthCookie(string cookieName);
}