// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.070
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class Proceso : Entity, IAggregateRoot
{
    protected Proceso() { }

    public Proceso(
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
    }

    public Guid IdMacro_Proceso { get; private set; } = Guid.Empty;
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string Contexto { get; private set; } = string.Empty;
    public string NivelDeProceso { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;
    public string Token { get; private set; } = string.Empty;
    public string ComoDesplegarUrlDeProceso { get; private set; } = string.Empty;
    public bool ProcesoBase { get; private set; } = false;
    public int MaximaAsignacionDeRoles { get; private set; } = 1;
    public bool Activo { get; private set; } = true;

    public void CambiarProcesoBase(bool nuevoValor)
    {
        ProcesoBase = nuevoValor;
    }

    public void CambiarMaximaAsignacionDeRoles(int nuevoValor)
    {
        MaximaAsignacionDeRoles = nuevoValor;
    }

    public void CambiarActivo(bool nuevoValor)
    {
        Activo = nuevoValor;
    }
}

