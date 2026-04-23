using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Auditing;

public sealed class EfAuditJournalReader(AUT2ServicesContext dbContext) : IAuditJournalReader
{
    public async Task<IReadOnlyList<AuditEnvelope>> GetAggregateTimelineAsync(Guid aggregateId, CancellationToken cancellationToken = default)
    {
        var messages = await dbContext.AuditOutboxMessages
            .AsNoTracking()
            .Include(message => message.Changes)
            .Where(message => message.AggregateId == aggregateId)
            .OrderBy(message => message.AggregateRevision)
            .ThenBy(message => message.OccurredAtUtc)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return messages.Select(Map).ToArray();
    }

    public async Task<AuditEnvelope?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.AuditOutboxMessages
            .AsNoTracking()
            .Include(item => item.Changes)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return message is null ? null : Map(message);
    }

    private static AuditEnvelope Map(AuditOutboxMessage message)
    {
        return new AuditEnvelope
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
            Actor = new AuditActor(
                message.ActorUserId,
                message.ActorUsername,
                message.ActorEmail),
            SnapshotJson = message.SnapshotJson,
            Changes = message.Changes
                .OrderBy(change => change.Order)
                .Select(change => new AuditDeltaChange(
                    change.Order,
                    change.Path,
                    change.ValueType,
                    change.NewValueJson))
                .ToArray()
        };
    }
}
