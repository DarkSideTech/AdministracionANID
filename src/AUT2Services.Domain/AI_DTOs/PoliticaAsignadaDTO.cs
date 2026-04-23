// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.073
// -------------------------------------------------
namespace AUT2Services.Domain.DTOs;

public class PoliticaAsignadaDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid Id_Entidad { get; set; } = Guid.Empty;
    public Guid Id_Rol { get; set; } = Guid.Empty;
    public Guid Id_Proceso { get; set; } = Guid.Empty;
    public DateTimeOffset? FechaInicioAsignacion { get; set; }
    public DateTimeOffset? FechaTerminoAsignacion { get; set; }
    public DateTimeOffset? FechaCreacion { get; set; }
    public bool RolRequiereValidacion { get; set; } = false;
    public bool RolAsignadoValidado { get; set; } = false;
    public bool PoliticaAsignadaBase { get; set; } = false;
}

