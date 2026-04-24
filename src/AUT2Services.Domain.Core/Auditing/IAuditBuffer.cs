using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditBuffer
{
    void TrackCreate<TAggregate>(Command command, Event domainEvent, TAggregate after);
    void TrackUpdate<TAggregate>(Command command, Event domainEvent, TAggregate before, TAggregate after, bool includeSnapshot = false);
    void TrackDelete<TAggregate>(Command command, Event domainEvent, TAggregate before);
    IReadOnlyList<PendingAuditEntry> Drain();
}
