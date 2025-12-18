// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.089
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Procesos.Events;

public class ProcesoEventCreado : Event
{
    public ProcesoEventCreado(
        Guid id, 
            Guid idMacro_Proceso, 
            string codigo, 
            string nombre, 
            string descripcion, 
            string contexto, 
            string nivelDeProceso, 
            string url, 
            string token, 
            string comoDesplegarUrlDeProceso, 
            bool procesoBase, 
            int maximaAsignacionDeRoles, 
            bool activo 
        )
    {
        Id = id;
        IdMacro_Proceso = idMacro_Proceso; 
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
        Contexto = contexto; 
        NivelDeProceso = nivelDeProceso; 
        Url = url; 
        Token = token; 
        ComoDesplegarUrlDeProceso = comoDesplegarUrlDeProceso; 
        ProcesoBase = procesoBase; 
        MaximaAsignacionDeRoles = maximaAsignacionDeRoles; 
        Activo = activo; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid IdMacro_Proceso  { get; private set; } = Guid.Empty; 
    public string Codigo  { get; private set; } = string.Empty; 
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
    public string Contexto  { get; private set; } = string.Empty; 
    public string NivelDeProceso  { get; private set; } = string.Empty; 
    public string Url  { get; private set; } = string.Empty; 
    public string Token  { get; private set; } = string.Empty; 
    public string ComoDesplegarUrlDeProceso  { get; private set; } = string.Empty; 
    public bool ProcesoBase  { get; private set; } = false; 
    public int MaximaAsignacionDeRoles  { get; private set; } = 1; 
    public bool Activo  { get; private set; } = true; 
}

