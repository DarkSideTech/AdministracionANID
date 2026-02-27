// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.259
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales
{
    public class UnidadOrganizacionalCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public Guid Id_Organizacion { get; protected set; } = Guid.Empty; 
        public string Codigo { get; protected set; } = string.Empty; 
        public string Nombre { get; protected set; } = string.Empty; 
        public string Descripcion { get; protected set; } = string.Empty; 
        public bool UnidadOrganizacionalBase { get; protected set; } = false; 
        public bool Activo { get; protected set; } = true; 
    }
}

