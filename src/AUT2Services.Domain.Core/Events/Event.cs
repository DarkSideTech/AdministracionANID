using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Time;

namespace AUT2Services.Domain.Core.Events;

public abstract class Event : Message, INotification
{
    public DateTimeOffset Timestamp { get; private set; }
    public long AggregateRevision { get; private set; }

    protected Event()
    {
        Timestamp = ClockContext.Current.UtcNow;
    }

    internal void SetAggregateRevision(long revision)
    {
        AggregateRevision = revision;
    }
}
