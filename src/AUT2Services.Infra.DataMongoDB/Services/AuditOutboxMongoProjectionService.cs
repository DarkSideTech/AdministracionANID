using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataMongoDB.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.DataMongoDB.Services;

public sealed class AuditOutboxMongoProjectionService : BackgroundService
{
    private const short PendingDispatchStatus = 0;
    private const short ProcessedDispatchStatus = 1;

    private readonly IServiceScopeFactory scopeFactory;
    private readonly IOptions<MongoAuditProjectionOptions> options;
    private readonly IClock clock;
    private readonly ILogger<AuditOutboxMongoProjectionService> logger;

    public AuditOutboxMongoProjectionService(
        IServiceScopeFactory scopeFactory,
        IOptions<MongoAuditProjectionOptions> options,
        IClock clock,
        ILogger<AuditOutboxMongoProjectionService> logger)
    {
        this.scopeFactory = scopeFactory;
        this.options = options;
        this.clock = clock;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentOptions = options.Value;
        if (!currentOptions.Enabled)
        {
            logger.LogInformation("Mongo audit projection disabled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(currentOptions.ConnectionString) || string.IsNullOrWhiteSpace(currentOptions.DatabaseName))
        {
            logger.LogWarning("Mongo audit projection enabled but Mongo connection settings are incomplete.");
            return;
        }

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(1, currentOptions.IntervalSeconds)));

        await ProjectPendingAuditEntriesAsync(stoppingToken).ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            await ProjectPendingAuditEntriesAsync(stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task ProjectPendingAuditEntriesAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AUT2ServicesContext>();
        var projectionWriter = scope.ServiceProvider.GetRequiredService<IAuditProjectionWriter>();
        var currentOptions = options.Value;

        var pendingMessages = await dbContext.AuditOutboxMessages
            .Include(message => message.Changes)
            .Where(message => message.DispatchStatus == PendingDispatchStatus)
            .OrderBy(message => message.PersistedAtUtc)
            .ThenBy(message => message.Id)
            .Take(Math.Max(1, currentOptions.BatchSize))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (pendingMessages.Count == 0)
        {
            return;
        }

        var attemptAtUtc = clock.UtcNow;

        try
        {
            var envelopes = pendingMessages
                .Select(message => new AuditEnvelope
                {
                    Id = message.Id,
                    CorrelationId = message.CorrelationId,
                    AggregateId = message.AggregateId,
                    AggregateType = message.AggregateType,
                    AggregateRevision = message.AggregateRevision,
                    EventType = message.EventType,
                    CommandType = message.CommandType,
                    OperationType = message.OperationType,
                    OccurredAtUtc = message.OccurredAtUtc,
                    PersistedAtUtc = message.PersistedAtUtc,
                    RequestPath = message.RequestPath,
                    Actor = new AuditActor(message.ActorUserId, message.ActorUsername, message.ActorEmail),
                    SnapshotJson = message.SnapshotJson,
                    Changes = message.Changes
                        .OrderBy(change => change.Order)
                        .Select(change => new AuditDeltaChange(change.Order, change.Path, change.ValueType, change.NewValueJson))
                        .ToArray()
                })
                .ToArray();

            await projectionWriter.WriteAsync(envelopes, cancellationToken).ConfigureAwait(false);

            foreach (var pendingMessage in pendingMessages)
            {
                pendingMessage.DispatchStatus = ProcessedDispatchStatus;
                pendingMessage.DispatchAttempts += 1;
                pendingMessage.LastDispatchAttemptUtc = attemptAtUtc;
                pendingMessage.DispatchedAtUtc = attemptAtUtc;
                pendingMessage.LastError = null;
            }

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error projecting audit outbox batch to MongoDB.");

            foreach (var pendingMessage in pendingMessages)
            {
                pendingMessage.DispatchAttempts += 1;
                pendingMessage.LastDispatchAttemptUtc = attemptAtUtc;
                pendingMessage.LastError = ex.Message;
            }

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
