using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace AUT2Services.Infra.Security.Services;

public class CsrfService(
    IOptions<JwtOptions> jwtOptions,
    IClock clock) : ICsrfService
{
    private readonly JwtOptions jwtOptions = jwtOptions.Value;

    public void EnsureTokenCookie(HttpResponse response, string? existingToken = null)
    {
        var token = string.IsNullOrWhiteSpace(existingToken)
            ? Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32))
            : existingToken;

        response.Cookies.Append(
            EnumCsrfNames.Cookie,
            token,
            new CookieOptions
            {
                HttpOnly = false,
                Secure = jwtOptions.ImplementCookieOptionsSecure,
                SameSite = SameSiteMode.Strict,
                IsEssential = true,
                Path = "/",
                Expires = clock.UtcNow.AddHours(8)
            });
    }

    public bool IsRequestValid(HttpRequest request)
    {
        if (!request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var cookieToken))
        {
            return false;
        }

        if (!request.Headers.TryGetValue(EnumCsrfNames.Header, out var headerToken))
        {
            return false;
        }

        var cookieBytes = Encoding.UTF8.GetBytes(cookieToken);
        var headerBytes = Encoding.UTF8.GetBytes(headerToken.ToString());

        if (cookieBytes.Length != headerBytes.Length)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(cookieBytes, headerBytes);
    }
}