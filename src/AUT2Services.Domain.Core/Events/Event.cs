using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;

namespace AUT2Services.Domain.Core.Events;
public abstract class Event : Message, INotification
{
    public DateTimeOffset Timestamp { get; private set; }

    protected Event()
    {
        Timestamp = DateTimeOffset.Now;
    }
}
