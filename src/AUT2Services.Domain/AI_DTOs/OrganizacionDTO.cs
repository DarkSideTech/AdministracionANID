// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.072
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class OrganizacionDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public string IdOrganizacion { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool OrganizacionBase { get; set; } = false;
    public bool Activo { get; set; } = true;
}

