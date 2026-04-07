using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Services;

public class ExpiredRefreshTokenCleanupService(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<ExpiredRefreshTokenCleanupService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalMinutes = Math.Max(5, configuration.GetValue<int?>("RefreshTokenCleanup:IntervalMinutes") ?? 60);
        var timer = new PeriodicTimer(TimeSpan.FromMinutes(intervalMinutes));

        await CleanupExpiredTokensAsync(stoppingToken);

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await CleanupExpiredTokensAsync(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AUT2ServicesContext>();
            var nowUtc = DateTime.UtcNow;

            var expiredTokens = await dbContext.RefreshTokens
                .Where(x => x.ExpiresAtUtc <= nowUtc)
                .ToListAsync(cancellationToken);

            if (expiredTokens.Count == 0)
            {
                return;
            }

            dbContext.RefreshTokens.RemoveRange(expiredTokens);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"Se eliminaron {expiredTokens.Count} tokens de actualización caducados.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudieron limpiar los tokens de actualización caducados.");
        }
    }
}
