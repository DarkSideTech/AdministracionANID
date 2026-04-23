// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.128
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos
{
    public class ValidacionEnrrolamientoCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public Guid IdValidado_Usuario { get; protected set; } = Guid.Empty; 
        public Guid IdValidaEnrrolamiento_Usuario { get; protected set; } = Guid.Empty; 
        public bool EnrrolamientoAceptado { get; protected set; } = false; 
        public DateTimeOffset? FechaValidacion { get; protected set; }
        public DateTimeOffset? FechaRegistro { get; protected set; }
        public bool Activo { get; protected set; } = true; 
    }
}

