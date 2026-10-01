namespace AUT2Services.Infra.DataTrazabilidad.Auditing;

public static class AuditSensitiveDataPolicy
{
    public const string RedactedJsonValue = "\"[REDACTED]\"";

    private static readonly HashSet<string> SensitivePropertyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Token",
        "ValidationToken",
        "ValidationCode",
        "Password",
        "PasswordHash",
        "AccessToken",
        "RefreshToken",
        "ClientSecret",
        "SecurityStamp",
        "CodeHash"
    };

    public static bool IsSensitiveProperty(string? propertyName)
        => !string.IsNullOrWhiteSpace(propertyName)
           && SensitivePropertyNames.Contains(propertyName);
}
