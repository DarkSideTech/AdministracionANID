using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;

namespace AUT2Services.Domain.Core.Commands
{
    public abstract class Command : Message, IRequest<CommandResponse>
    {
        public CommandResponse CommandResponse { get; set; }

        protected Command()
        {
            CommandResponse = new CommandResponse();
        }

        public virtual bool IsValid()
        {
            return CommandResponse.ValidationResult.IsValid;
        }
    }
}
