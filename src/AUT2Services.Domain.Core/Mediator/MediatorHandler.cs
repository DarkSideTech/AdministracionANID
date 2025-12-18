using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;
using System.Runtime.CompilerServices;

namespace AUT2Services.Domain.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Task<CommandResponse> SendCommand<T>(T command) where T : Command
        {

            return _mediator.Send(command);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<CommandResponse> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command
        {
            return _mediator.Send(command, cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Task PublishEvent<T>(T @event) where T : Event
        {
            return _mediator.Publish(@event);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual Task PublishEvent<T>(T @event, CancellationToken cancellationToken) where T : Event
        {
            return _mediator.Publish(@event, cancellationToken);
        }
    }
}
