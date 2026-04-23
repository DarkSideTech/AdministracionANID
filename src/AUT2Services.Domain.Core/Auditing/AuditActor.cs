namespace AUT2Services.Domain.Core.Auditing;

public sealed record AuditActor(
    string? UserId,
    string? Username,
    string? Email);
