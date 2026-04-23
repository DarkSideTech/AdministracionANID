// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.071
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class ValidacionEnrrolamientoDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid IdValidado_Usuario { get; set; } = Guid.Empty;
    public Guid IdValidaEnrrolamiento_Usuario { get; set; } = Guid.Empty;
    public bool EnrrolamientoAceptado { get; set; } = false;
    public DateTimeOffset? FechaValidacion { get; set; }
    public DateTimeOffset? FechaRegistro { get; set; }
    public bool Activo { get; set; } = true;
}

