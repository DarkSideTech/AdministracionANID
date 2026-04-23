namespace AUT2Services.Domain.Core.Auditing;

public interface IAuditSerializer
{
    string? Serialize(object? value);
}
