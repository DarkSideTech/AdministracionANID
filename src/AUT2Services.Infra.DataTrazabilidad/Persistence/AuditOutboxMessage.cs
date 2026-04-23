using AUT2Services.Domain.Core.Auditing;

namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class AuditOutboxMessage
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid AggregateId { get; set; }
    public string AggregateType { get; set; } = string.Empty;
    public long AggregateRevision { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string CommandType { get; set; } = string.Empty;
    public AuditOperationType OperationType { get; set; }
    public string? ActorUserId { get; set; }
    public string? ActorUsername { get; set; }
    public string? ActorEmail { get; set; }
    public string? RequestPath { get; set; }
    public DateTimeOffset OccurredAtUtc { get; set; }
    public DateTimeOffset PersistedAtUtc { get; set; }
    public string? SnapshotJson { get; set; }
    public short DispatchStatus { get; set; }
    public int DispatchAttempts { get; set; }
    public DateTimeOffset? LastDispatchAttemptUtc { get; set; }
    public DateTimeOffset? DispatchedAtUtc { get; set; }
    public string? LastError { get; set; }
    public short SchemaVersion { get; set; }
    public ICollection<AuditOutboxChange> Changes { get; set; } = [];
}
