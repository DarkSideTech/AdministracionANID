// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.122
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos
{
    public class AutenticadorExternoCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public Guid Id_Proveedor { get; protected set; } = Guid.Empty; 
        public Guid Id_Usuario { get; protected set; } = Guid.Empty; 
        public string NombreUsuario { get; protected set; } = string.Empty; 
        public string ClaveDeAcceso { get; protected set; } = string.Empty; 
        public string NombreADesplegar { get; protected set; } = string.Empty; 
        public bool ValidadorPrimario { get; protected set; } = false; 
        public bool AutenticadorExternoBase { get; protected set; } = false; 
        public bool Activo { get; protected set; } = true; 
    }
}

