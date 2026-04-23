namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class TraceabilityEventDetail
{
    public Guid Id { get; init; }
    public Guid CorrelationId { get; init; }
    public Guid AggregateId { get; init; }
    public string AggregateType { get; init; } = string.Empty;
    public long AggregateRevision { get; init; }
    public string EventType { get; init; } = string.Empty;
    public string CommandType { get; init; } = string.Empty;
    public string OperationType { get; init; } = string.Empty;
    public DateTimeOffset OccurredAtUtc { get; init; }
    public DateTimeOffset PersistedAtUtc { get; init; }
    public string? RequestPath { get; init; }
    public string? SnapshotJson { get; init; }
    public TraceabilityActorDto Actor { get; init; } = new();
    public IReadOnlyCollection<TraceabilityChangeItem> Changes { get; init; } = [];
}
