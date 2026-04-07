// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.152
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Domain.Commands.Procesos
{
    public class ProcesoCommand : Command
    {
        public Guid Id { get; protected set; } = Guid.Empty; 
        public Guid IdMacro_Proceso { get; protected set; } = Guid.Empty; 
        public string Codigo { get; protected set; } = string.Empty; 
        public string Nombre { get; protected set; } = string.Empty; 
        public string Descripcion { get; protected set; } = string.Empty; 
        public string Contexto { get; protected set; } = string.Empty; 
        public string NivelDeProceso { get; protected set; } = string.Empty; 
        public string Url { get; protected set; } = string.Empty; 
        public string Token { get; protected set; } = string.Empty; 
        public string ComoDesplegarUrlDeProceso { get; protected set; } = string.Empty; 
        public bool ProcesoBase { get; protected set; } = false; 
        public int MaximaAsignacionDeRoles { get; protected set; } = 1; 
        public bool Activo { get; protected set; } = true; 
    }
}

