namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditExecutionContextInitializer
{
    void Initialize(
        Guid correlationId,
        DateTimeOffset requestedAtUtc,
        string? userId,
        string? username,
        string? email,
        string? requestPath);
}
