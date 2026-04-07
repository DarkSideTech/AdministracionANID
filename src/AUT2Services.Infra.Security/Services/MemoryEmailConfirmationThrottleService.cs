using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.Services;

public sealed class MemoryEmailConfirmationThrottleService(
    IMemoryCache memoryCache,
    IOptions<EmailValidationOptions> options) : IEmailConfirmationThrottleService
{
    public bool CanSend(string email, out DateTimeOffset? nextAllowedAtUtc)
    {
        var normalizedEmail = email.Trim().ToUpperInvariant();
        var cacheKey = $"email-confirmation:{normalizedEmail}";

        if (memoryCache.TryGetValue<DateTimeOffset>(cacheKey, out var nextAllowed))
        {
            nextAllowedAtUtc = nextAllowed;
            return false;
        }

        var cooldown = TimeSpan.FromMinutes(Math.Max(1, options.Value.ResendCooldownMinutes));
        nextAllowedAtUtc = DateTimeOffset.UtcNow.Add(cooldown);

        memoryCache.Set(cacheKey, nextAllowedAtUtc.Value, nextAllowedAtUtc.Value);
        return true;
    }
}