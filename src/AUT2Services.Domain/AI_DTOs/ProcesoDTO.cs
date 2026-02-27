// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.213
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class ProcesoDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid IdMacro_Proceso { get; set; } = Guid.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Contexto { get; set; } = string.Empty;
    public string NivelDeProceso { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string ComoDesplegarUrlDeProceso { get; set; } = string.Empty;
    public bool ProcesoBase { get; set; } = false;
    public int MaximaAsignacionDeRoles { get; set; } = 1;
    public bool Activo { get; set; } = true;
}

