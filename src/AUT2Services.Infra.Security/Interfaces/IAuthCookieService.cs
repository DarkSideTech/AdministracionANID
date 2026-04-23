using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Interfaces;

public interface IAuthCookieService
{
    void AppendAuthCookies(HttpResponse response, AccessTokenResult accessToken, DateTimeOffset? refreshTokenExpiresAtUtc, string refreshToken);
    void ClearAuthCookies(HttpResponse response);
}
