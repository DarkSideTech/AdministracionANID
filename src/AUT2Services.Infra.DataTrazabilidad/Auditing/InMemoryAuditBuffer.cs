using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Infra.DataTrazabilidad.Auditing;

public sealed class InMemoryAuditBuffer : IAuditBuffer
{
    private readonly List<PendingAuditEntry> entries = [];
    private int captureOrder;

    public void TrackCreate<TAggregate>(Command command, Event domainEvent, TAggregate after)
    {
        entries.Add(new PendingAuditEntry
        {
            CommandType = command.MessageType,
            DomainEvent = domainEvent,
            AggregateType = typeof(TAggregate).Name,
            OperationType = AuditOperationType.Create,
            After = after,
            CaptureOrder = captureOrder++
        });
    }

    public void TrackUpdate<TAggregate>(Command command, Event domainEvent, TAggregate before, TAggregate after)
    {
        entries.Add(new PendingAuditEntry
        {
            CommandType = command.MessageType,
            DomainEvent = domainEvent,
            AggregateType = typeof(TAggregate).Name,
            OperationType = AuditOperationType.Update,
            Before = before,
            After = after,
            CaptureOrder = captureOrder++
        });
    }

    public void TrackDelete<TAggregate>(Command command, Event domainEvent, TAggregate before)
    {
        entries.Add(new PendingAuditEntry
        {
            CommandType = command.MessageType,
            DomainEvent = domainEvent,
            AggregateType = typeof(TAggregate).Name,
            OperationType = AuditOperationType.Delete,
            Before = before,
            CaptureOrder = captureOrder++
        });
    }

    public IReadOnlyList<PendingAuditEntry> Drain()
    {
        if (entries.Count == 0)
        {
            return [];
        }

        var drained = entries
            .OrderBy(entry => entry.CaptureOrder)
            .ToArray();

        entries.Clear();
        captureOrder = 0;

        return drained;
    }
}
