using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.Services;

public class AuthCookieService(IClock clock, IOptions<JwtOptions> jwtOptions) : IAuthCookieService
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;

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
        Secure = jwtOptions.ImplementCookieOptionsSecure,
        SameSite = SameSiteMode.Strict,
        Expires = expiresAtUtc,
        IsEssential = true,
        Path = "/"
    };

    private CookieOptions BuildDeleteCookieOptions() => BuildCookieOptions(clock.UtcNow.AddDays(-1));
}
