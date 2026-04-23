namespace AUT2Services.Domain.Core.Auditing;

public sealed record AuditDeltaChange(
    int Order,
    string Path,
    string? ValueType,
    string? NewValueJson);
