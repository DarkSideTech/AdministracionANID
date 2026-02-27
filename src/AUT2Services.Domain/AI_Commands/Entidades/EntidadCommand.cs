// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.264
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.Entidades
{
    public class EntidadCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public Guid Id_UnidadOrganizacional { get; protected set; } = Guid.Empty; 
        public Guid Id_Usuario { get; protected set; } = Guid.Empty; 
        public string TipoDeEntidad { get; protected set; } = string.Empty; 
        public string CorreoElectronico { get; protected set; } = string.Empty; 
        public DateTimeOffset FechaInicioAutorizacion { get; protected set; } = DateTimeOffset.MinValue; 
        public DateTimeOffset FechaTerminoAutorizacion { get; protected set; } = DateTimeOffset.MinValue; 
        public DateTimeOffset FechaCreacion { get; protected set; } = DateTimeOffset.MinValue; 
        public bool Principal { get; protected set; } = false; 
        public bool EntidadBase { get; protected set; } = true; 
    }
}

