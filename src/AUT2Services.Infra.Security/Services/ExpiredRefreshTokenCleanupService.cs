using AUT2Services.Domain.Core.Time;
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
    ILogger<ExpiredRefreshTokenCleanupService> logger,
    IClock clock) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalMinutes = Math.Max(5, configuration.GetValue<int?>("RefreshTokenCleanup:IntervalMinutes") ?? 60);
        logger.LogInformation("Iniciando limpieza de refresh tokens expirados con un intervalo de {IntervalMinutes} minutos.", intervalMinutes);

        await CleanupExpiredTokensAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(intervalMinutes));

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
            var nowUtc = clock.UtcNow;

            var deletedCount = await dbContext.RefreshTokens
                .Where(x => x.ExpiresAtUtc != null && x.ExpiresAtUtc <= nowUtc)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedCount == 0)
            {
                logger.LogDebug("No se encontraron refresh tokens expirados para eliminar.");
                return;
            }

            logger.LogInformation("Se eliminaron {DeletedCount} refresh tokens expirados.", deletedCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudieron limpiar los tokens de actualización caducados.");
        }
    }
}

