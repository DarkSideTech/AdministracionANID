using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Security.Services;

public class SessionValidationService(
    AUT2ServicesContext dbContext,
    UserManager<Usuario> userManager) : ISessionValidationService
{
    public async Task<bool> IsSessionValidAsync(string? userId, string? sessionId, string? securityStamp, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sessionId))
        {
            return false;
        }

        var sessionExists = await dbContext.RefreshTokens.AnyAsync(
            x => x.UserId == userId &&
                 x.SessionId == sessionId &&
                 x.RevokedAtUtc == null &&
                 x.ExpiresAtUtc > DateTime.UtcNow,
            cancellationToken);

        if (!sessionExists)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(securityStamp))
        {
            return true;
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }

        var currentSecurityStamp = await userManager.GetSecurityStampAsync(user);
        return string.Equals(currentSecurityStamp, securityStamp, StringComparison.Ordinal);
    }
}