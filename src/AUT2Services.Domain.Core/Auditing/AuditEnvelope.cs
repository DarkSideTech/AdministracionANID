namespace AUT2Services.Domain.Core.Auditing;

public sealed class AuditEnvelope
{
    public Guid Id { get; init; }
    public Guid CorrelationId { get; init; }
    public Guid AggregateId { get; init; }
    public string AggregateType { get; init; } = string.Empty;
    public long AggregateRevision { get; init; }
    public string EventType { get; init; } = string.Empty;
    public string CommandType { get; init; } = string.Empty;
    public AuditOperationType OperationType { get; init; }
    public DateTimeOffset OccurredAtUtc { get; init; }
    public DateTimeOffset PersistedAtUtc { get; init; }
    public string? RequestPath { get; init; }
    public AuditActor Actor { get; init; } = new(null, null, null);
    public string? SnapshotJson { get; init; }
    public IReadOnlyCollection<AuditDeltaChange> Changes { get; init; } = [];
}
