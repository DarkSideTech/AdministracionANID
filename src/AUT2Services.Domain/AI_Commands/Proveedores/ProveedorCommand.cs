// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.110
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.Proveedores
{
    public class ProveedorCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public string Codigo { get; protected set; } = string.Empty; 
        public string Nombre { get; protected set; } = string.Empty; 
        public string Descripcion { get; protected set; } = string.Empty; 
        public string APIDeAutenticacion { get; protected set; } = string.Empty; 
        public bool ProveedorBase { get; protected set; } = false; 
        public bool Activo { get; protected set; } = true; 
    }
}

