// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.112
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.Organizaciones
{
    public class OrganizacionCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public string IdOrganizacion { get; protected set; } = string.Empty; 
        public string Codigo { get; protected set; } = string.Empty; 
        public string Nombre { get; protected set; } = string.Empty; 
        public string Descripcion { get; protected set; } = string.Empty; 
        public bool OrganizacionBase { get; protected set; } = false; 
        public bool Activo { get; protected set; } = true; 
    }
}

