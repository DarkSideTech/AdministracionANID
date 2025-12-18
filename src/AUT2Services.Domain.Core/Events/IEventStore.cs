using AUT2Services.Domain.Core.Messaging;

namespace AUT2Services.Domain.Core.Events
{
    public interface IEventStore
    {
        void Save<T>(T theEvent) where T : Event;
    }
}
