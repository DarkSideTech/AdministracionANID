namespace AUT2Services.Domain.DTOs;

public class ProcesoActivoDTO
{
    public string Codigo { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = [];
    public string NombreProceso { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string ComoDesplegarUrlDeProceso { get; set; } = string.Empty;
}
