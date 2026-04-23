namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditDeltaBuilder
{
    IReadOnlyCollection<AuditDeltaChange> Build(object? before, object? after);
}
