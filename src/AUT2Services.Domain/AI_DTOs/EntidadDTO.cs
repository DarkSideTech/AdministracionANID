// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.077
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class EntidadDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid Id_UnidadOrganizacional { get; set; } = Guid.Empty;
    public Guid Id_Usuario { get; set; } = Guid.Empty;
    public string TipoDeEntidad { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public DateTimeOffset FechaInicioAutorizacion { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset FechaTerminoAutorizacion { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.MinValue;
    public bool Principal { get; set; } = false;
    public bool EntidadBase { get; set; } = true;
}

