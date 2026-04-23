using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Traceability;

public sealed class SecurityTraceabilityCommand : Command
{
    public SecurityTraceabilityCommand(Guid aggregateId, string commandType)
    {
        AggregateId = aggregateId;
        MessageType = string.IsNullOrWhiteSpace(commandType)
            ? nameof(SecurityTraceabilityCommand)
            : commandType.Trim();
    }
}
