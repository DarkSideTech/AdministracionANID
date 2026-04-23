using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Domain.Core.Auditing;

public sealed class NoOpAuditBuffer : IAuditBuffer
{
    public static readonly NoOpAuditBuffer Instance = new();

    private NoOpAuditBuffer()
    {
    }

    public void TrackCreate<TAggregate>(Command command, Event domainEvent, TAggregate after)
    {
    }

    public void TrackUpdate<TAggregate>(Command command, Event domainEvent, TAggregate before, TAggregate after)
    {
    }

    public void TrackDelete<TAggregate>(Command command, Event domainEvent, TAggregate before)
    {
    }

    public IReadOnlyList<PendingAuditEntry> Drain()
    {
        return [];
    }
}
