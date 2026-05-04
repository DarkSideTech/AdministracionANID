namespace AUT2Services.Domain.DTOs;

public sealed class UnidadOrganizacionalAsignacionOrganizacionDTO
{
    public Guid IdUnidadOrganizacional { get; set; }
    public Guid IdOrganizacionActual { get; set; }
    public string CodigoUnidadOrganizacional { get; set; } = string.Empty;
    public string NombreUnidadOrganizacional { get; set; } = string.Empty;
    public string DescripcionUnidadOrganizacional { get; set; } = string.Empty;
    public bool UnidadOrganizacionalBase { get; set; }
    public bool Activo { get; set; }
    public string CodigoOrganizacionActual { get; set; } = string.Empty;
    public string NombreOrganizacionActual { get; set; } = string.Empty;
    public bool AsignadaAOrganizacion { get; set; }
    public bool TieneEntidadPrincipal { get; set; }
    public long CantidadEntidadesPrincipales { get; set; }
}
