using AUT2Services.Domain.Core.Commands;
using AUT2Services.Infra.Security.Traceability;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ISecurityTraceabilityService
{
    bool TrackCreate(Command command, string? userId, string eventType, UsuarioTraceabilityState state);
    bool TrackCreate(string commandType, string? userId, string eventType, UsuarioTraceabilityState state);
    bool TrackUpdate(Command command, string? userId, string eventType, UsuarioTraceabilityState before, UsuarioTraceabilityState after);
}
