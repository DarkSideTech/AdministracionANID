// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.077
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class ValidacionEnrrolamientoDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid IdValidado_Usuario { get; set; } = Guid.Empty;
    public Guid IdValidaEnrrolamiento_Usuario { get; set; } = Guid.Empty;
    public bool EnrrolamientoAceptado { get; set; } = false;
    public DateTimeOffset FechaValidacion { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset FechaRegistro { get; set; } = DateTimeOffset.MinValue;
    public bool Activo { get; set; } = true;
}

