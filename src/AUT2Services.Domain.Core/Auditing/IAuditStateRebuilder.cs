namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditStateRebuilder
{
    Task<string?> RebuildStateJsonAsync(Guid aggregateId, long targetRevision, CancellationToken cancellationToken = default);
}
