namespace AUT2Services.Domain.DTOs;

public class ProcesoActivoDTO
{
    public Guid IdProceso { get; set; } = Guid.Empty;
    public Guid IdMacroProceso { get; set; } = Guid.Empty;
    public string Codigo { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = [];
    public string NombreProceso { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ComoDesplegarUrlDeProceso { get; set; } = string.Empty;
}
