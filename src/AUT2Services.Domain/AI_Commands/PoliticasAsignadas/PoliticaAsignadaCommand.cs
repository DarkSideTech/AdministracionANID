// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.116
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas
{
    public class PoliticaAsignadaCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public Guid Id_Entidad { get; protected set; } = Guid.Empty; 
        public Guid Id_Rol { get; protected set; } = Guid.Empty; 
        public Guid Id_Proceso { get; protected set; } = Guid.Empty; 
        public DateTimeOffset FechaInicioAsignacion { get; protected set; } = DateTimeOffset.MinValue; 
        public DateTimeOffset FechaTerminoAsignacion { get; protected set; } = DateTimeOffset.MinValue; 
        public DateTimeOffset FechaCreacion { get; protected set; } = DateTimeOffset.MinValue; 
        public bool RolRequiereValidacion { get; protected set; } = false; 
        public bool RolAsignadoValidado { get; protected set; } = false; 
        public bool PoliticaAsignadaBase { get; protected set; } = false; 
    }
}

