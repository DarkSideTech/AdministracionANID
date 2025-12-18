// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.076
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class ProveedorDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string APIDeAutenticacion { get; set; } = string.Empty;
    public bool ProveedorBase { get; set; } = false;
    public bool Activo { get; set; } = true;
}

