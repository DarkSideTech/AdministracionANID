namespace AUT2Services.Domain.DTOs;

public class EntidadParaAsignarPoliticaDTO
{
    public Guid IdEntidad { get; set; } = Guid.Empty;
    public Guid IdUsuario { get; set; } = Guid.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public Guid IdUnidadOrganizacional { get; set; } = Guid.Empty;
    public string CodigoUnidadOrganizacional { get; set; } = string.Empty;
    public string NombreUnidadOrganizacional { get; set; } = string.Empty;
    public string TipoDeEntidad { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public bool Principal { get; set; }
}
