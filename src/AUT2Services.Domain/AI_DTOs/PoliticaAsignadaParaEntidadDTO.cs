namespace AUT2Services.Domain.DTOs;

public class PoliticaAsignadaParaEntidadDTO
{
    public Guid IdPoliticaAsignada { get; set; } = Guid.Empty;
    public Guid IdEntidad { get; set; } = Guid.Empty;
    public Guid IdRol { get; set; } = Guid.Empty;
    public string NombreRol { get; set; } = string.Empty;
    public Guid IdProceso { get; set; } = Guid.Empty;
    public string CodigoProceso { get; set; } = string.Empty;
    public string NombreProceso { get; set; } = string.Empty;
    public bool RolRequiereValidacion { get; set; }
    public bool RolAsignadoValidado { get; set; }
}
