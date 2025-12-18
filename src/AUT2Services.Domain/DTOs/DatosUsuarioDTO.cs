namespace AUT2Services.Domain.DTOs;

public class DatosUsuarioDTO
{
    public string NombreADesplegar { get; set; } = string.Empty;
    public string CodigoOrganizaicon { get; set; } = string.Empty;
    public string NombreOrganizaicon { get; set; } = string.Empty;
    public string CodigoUnidadOrganizacional { get; set; } = string.Empty;
    public string NombreUnidadOrganizacional { get; set; } = string.Empty;
    public IEnumerable<ProcesoActivoDTO> ProcesosActivos { get; set; } = [];
}
