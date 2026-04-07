using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Services;

public class AuthCookieService() : IAuthCookieService
{
    public void AppendAuthCookies(HttpResponse response, AccessTokenResult accessToken, DateTime refreshTokenExpiresAtUtc, string refreshToken)
    {
        response.Cookies.Append(
            EnumAuthCookieNames.AccessToken,
            accessToken.Token,
            BuildCookieOptions(accessToken.ExpiresAtUtc)
        );

        response.Cookies.Append(
            EnumAuthCookieNames.RefreshToken,
            refreshToken,
            BuildCookieOptions(refreshTokenExpiresAtUtc)
        );
    }

    public void ClearAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete(EnumAuthCookieNames.AccessToken, BuildDeleteCookieOptions());
        response.Cookies.Delete(EnumAuthCookieNames.RefreshToken, BuildDeleteCookieOptions());
    }

    private CookieOptions BuildCookieOptions(DateTime expiresAtUtc) => new()
    {
        HttpOnly = true,
        Secure = false, //true para produccion
        SameSite = SameSiteMode.Strict,
        Expires = expiresAtUtc,
        IsEssential = true,
        Path = "/"
    };

    private CookieOptions BuildDeleteCookieOptions() => BuildCookieOptions(DateTime.UtcNow.AddDays(-1));
}
