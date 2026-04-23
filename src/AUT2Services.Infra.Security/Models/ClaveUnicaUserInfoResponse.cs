using System.Text.Json.Serialization;

namespace AUT2Services.Infra.Security.Models;

public sealed class ClaveUnicaUserInfoResponse
{
    [JsonPropertyName("sub")]
    public string Sub { get; set; } = string.Empty;

    [JsonPropertyName("RolUnico")]
    public ClaveUnicaRolUnicoPayload RolUnico { get; set; } = new();

    [JsonPropertyName("name")]
    public ClaveUnicaNamePayload Name { get; set; } = new();
}

public sealed class ClaveUnicaRolUnicoPayload
{
    [JsonPropertyName("numero")]
    public long? Numero { get; set; }

    [JsonPropertyName("DV")]
    public string Dv { get; set; } = string.Empty;

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;
}

public sealed class ClaveUnicaNamePayload
{
    [JsonPropertyName("nombres")]
    public string[] Nombres { get; set; } = [];

    [JsonPropertyName("apellidos")]
    public string[] Apellidos { get; set; } = [];
}
