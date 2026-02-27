// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.255
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
        public DateTimeOffset FechaValidacion { get; protected set; } = DateTimeOffset.MinValue; 
        public DateTimeOffset FechaRegistro { get; protected set; } = DateTimeOffset.MinValue; 
        public bool Activo { get; protected set; } = true; 
    }
}

