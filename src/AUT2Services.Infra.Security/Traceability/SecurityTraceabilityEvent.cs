using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Infra.Security.Traceability;

public sealed class SecurityTraceabilityEvent : Event
{
    public SecurityTraceabilityEvent(Guid aggregateId, string eventType)
    {
        AggregateId = aggregateId;
        MessageType = string.IsNullOrWhiteSpace(eventType)
            ? nameof(SecurityTraceabilityEvent)
            : eventType.Trim();
    }
}
