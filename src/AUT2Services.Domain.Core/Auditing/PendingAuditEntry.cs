using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Domain.Core.Auditing;

public sealed class PendingAuditEntry
{
    public string CommandType { get; init; } = string.Empty;
    public Event DomainEvent { get; init; } = default!;
    public string AggregateType { get; init; } = string.Empty;
    public AuditOperationType OperationType { get; init; }
    public object? Before { get; init; }
    public object? After { get; init; }
    public int CaptureOrder { get; init; }
}
