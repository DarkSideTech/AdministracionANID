using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Traceability;

namespace AUT2Services.Infra.Security.Services;

public sealed class SecurityTraceabilityService(IAuditBuffer auditBuffer) : ISecurityTraceabilityService
{
    private readonly IAuditBuffer auditBuffer = auditBuffer;

    public bool TrackCreate(Command command, string? userId, string eventType, UsuarioTraceabilityState state)
    {
        if (!TryCreateAggregateId(userId, out var aggregateId))
        {
            return false;
        }

        auditBuffer.TrackCreate(
            command,
            new SecurityTraceabilityEvent(aggregateId, eventType),
            state);
        return true;
    }

    public bool TrackCreate(string commandType, string? userId, string eventType, UsuarioTraceabilityState state)
    {
        if (!TryCreateAggregateId(userId, out var aggregateId))
        {
            return false;
        }

        auditBuffer.TrackCreate(
            new SecurityTraceabilityCommand(aggregateId, commandType),
            new SecurityTraceabilityEvent(aggregateId, eventType),
            state);
        return true;
    }

    public bool TrackUpdate(
        Command command,
        string? userId,
        string eventType,
        UsuarioTraceabilityState before,
        UsuarioTraceabilityState after,
        bool includeSnapshot = false)
    {
        if (!TryCreateAggregateId(userId, out var aggregateId))
        {
            return false;
        }

        auditBuffer.TrackUpdate(
            command,
            new SecurityTraceabilityEvent(aggregateId, eventType),
            before,
            after,
            includeSnapshot);
        return true;
    }

    private static bool TryCreateAggregateId(string? userId, out Guid aggregateId)
    {
        return Guid.TryParse(userId, out aggregateId) && aggregateId != Guid.Empty;
    }
}
