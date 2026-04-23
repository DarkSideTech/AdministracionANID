namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditProjectionWriter
{
    Task WriteAsync(IReadOnlyCollection<AuditEnvelope> envelopes, CancellationToken cancellationToken = default);
}
