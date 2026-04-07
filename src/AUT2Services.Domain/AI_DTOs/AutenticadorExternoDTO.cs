// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.071
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class AutenticadorExternoDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid Id_Proveedor { get; set; } = Guid.Empty;
    public Guid Id_Usuario { get; set; } = Guid.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string ClaveDeAcceso { get; set; } = string.Empty;
    public string NombreADesplegar { get; set; } = string.Empty;
    public bool ValidadorPrimario { get; set; } = false;
    public bool AutenticadorExternoBase { get; set; } = false;
    public bool Activo { get; set; } = true;
}

