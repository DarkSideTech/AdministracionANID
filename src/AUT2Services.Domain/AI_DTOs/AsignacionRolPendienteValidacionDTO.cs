namespace AUT2Services.Domain.DTOs;

public class AsignacionRolPendienteValidacionDTO
{
    public Guid IdPoliticaAsignada { get; set; } = Guid.Empty;
    public Guid IdEntidad { get; set; } = Guid.Empty;
    public Guid IdUsuario { get; set; } = Guid.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public Guid IdOrganizacion { get; set; } = Guid.Empty;
    public string CodigoOrganizacion { get; set; } = string.Empty;
    public string NombreOrganizacion { get; set; } = string.Empty;
    public Guid IdUnidadOrganizacional { get; set; } = Guid.Empty;
    public string CodigoUnidadOrganizacional { get; set; } = string.Empty;
    public string NombreUnidadOrganizacional { get; set; } = string.Empty;
    public string TipoDeEntidad { get; set; } = string.Empty;
    public Guid IdRol { get; set; } = Guid.Empty;
    public string NombreRol { get; set; } = string.Empty;
    public Guid IdProceso { get; set; } = Guid.Empty;
    public string CodigoProceso { get; set; } = string.Empty;
    public string NombreProceso { get; set; } = string.Empty;
    public DateTimeOffset? FechaCreacion { get; set; }
    public DateTimeOffset? FechaInicioAsignacion { get; set; }
    public DateTimeOffset? FechaTerminoAsignacion { get; set; }
    public bool RolRequiereValidacion { get; set; }
    public bool RolAsignadoValidado { get; set; }
}

public sealed record AsignacionesRolesPendientesValidacionPageDTO(
    int NumeroDePagina,
    int CantidadPorPagina,
    long Total,
    IReadOnlyCollection<AsignacionRolPendienteValidacionDTO> Items);
