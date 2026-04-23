namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditJournalReader
{
    Task<IReadOnlyList<AuditEnvelope>> GetAggregateTimelineAsync(Guid aggregateId, CancellationToken cancellationToken = default);
    Task<AuditEnvelope?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
