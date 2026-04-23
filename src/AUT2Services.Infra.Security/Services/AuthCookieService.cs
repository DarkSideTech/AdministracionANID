using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Services;

public class AuthCookieService(IClock clock) : IAuthCookieService
{
    public void AppendAuthCookies(HttpResponse response, AccessTokenResult accessToken, DateTimeOffset? refreshTokenExpiresAtUtc, string refreshToken)
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

    private CookieOptions BuildCookieOptions(DateTimeOffset? expiresAtUtc) => new()
    {
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.Strict,
        Expires = expiresAtUtc,
        IsEssential = true,
        Path = "/"
    };

    private CookieOptions BuildDeleteCookieOptions() => BuildCookieOptions(clock.UtcNow.AddDays(-1));
}