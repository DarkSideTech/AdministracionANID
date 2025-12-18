using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Domain.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T @event) where T : Event;
        Task PublishEvent<T>(T @event, CancellationToken cancellationToken) where T : Event;
        Task<CommandResponse> SendCommand<T>(T command) where T : Command;
        Task<CommandResponse> SendCommand<T>(T command, CancellationToken cancellationToken) where T : Command;
    }
}
