namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditExecutionContext
{
    Guid CorrelationId { get; }
    string? UserId { get; }
    string? Username { get; }
    string? Email { get; }
    string? RequestPath { get; }
    DateTimeOffset RequestedAtUtc { get; }
}
