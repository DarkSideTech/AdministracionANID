// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.213
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class UnidadOrganizacionalDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid Id_Organizacion { get; set; } = Guid.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool UnidadOrganizacionalBase { get; set; } = false;
    public bool Activo { get; set; } = true;
}

