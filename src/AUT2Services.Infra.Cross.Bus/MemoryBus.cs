using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.Core.Mediator;

namespace AUT2Services.Infra.Cross.Bus;

public sealed class MemoryBus : IMediatorHandler
{
    private readonly IMediator _mediator;
    //private readonly IEventStore _eventStore;

    //public MemoryBus(IEventStore eventStore, IMediator mediator)
    public MemoryBus(IMediator mediator)
    {
        //_eventStore = eventStore;
        _mediator = mediator;
    }

    public async Task PublishEvent<T>(T @event) where T : Event
    {
        //if (!@event.MessageType.Equals("DomainNotification"))
        //    _eventStore?.Save(@event);

        await _mediator.Publish(@event);
    }

    public async Task PublishEvent<T>(T @event, CancellationToken cancellationToken) where T : Event
    {
        await _mediator.Publish(@event, cancellationToken);
    }

    public async Task<CommandResponse> SendCommand<T>(T command) where T : Command
    {
        return await _mediator.Send(command);
    }

    public async Task<CommandResponse> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command
    {
        return await _mediator.Send(command, cancellationToken);
    }
}