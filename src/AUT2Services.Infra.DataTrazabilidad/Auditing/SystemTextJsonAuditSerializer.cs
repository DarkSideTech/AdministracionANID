using AUT2Services.Domain.Core.Auditing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AUT2Services.Infra.DataTrazabilidad.Auditing;

public sealed class SystemTextJsonAuditSerializer : IAuditSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public string? Serialize(object? value)
    {
        return value is null ? null : JsonSerializer.Serialize(value, SerializerOptions);
    }
}
