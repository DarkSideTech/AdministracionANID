// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.106
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Procesos.Events;

public class ProcesoEventModificado : Event
{
    public ProcesoEventModificado(
        Guid id, 
            string nombre, 
            string descripcion, 
            string contexto, 
            string nivelDeProceso, 
            string url, 
            string token, 
            string comoDesplegarUrlDeProceso, 
            bool procesoBase, 
            int maximaAsignacionDeRoles 
        )
    {
        Id = id;
        Nombre = nombre; 
        Descripcion = descripcion; 
        Contexto = contexto; 
        NivelDeProceso = nivelDeProceso; 
        Url = url; 
        Token = token; 
        ComoDesplegarUrlDeProceso = comoDesplegarUrlDeProceso; 
        ProcesoBase = procesoBase; 
        MaximaAsignacionDeRoles = maximaAsignacionDeRoles; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
    public string Contexto  { get; private set; } = string.Empty; 
    public string NivelDeProceso  { get; private set; } = string.Empty; 
    public string Url  { get; private set; } = string.Empty; 
    public string Token  { get; private set; } = string.Empty; 
    public string ComoDesplegarUrlDeProceso  { get; private set; } = string.Empty; 
    public bool ProcesoBase  { get; private set; } = false; 
    public int MaximaAsignacionDeRoles  { get; private set; } = 1; 
}

