// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.068
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class PoliticaAsignada : Entity, IAggregateRoot
{
    protected PoliticaAsignada() { }

    public PoliticaAsignada(
        Guid id, 
        Guid id_Entidad, 
        Guid id_Rol, 
        Guid id_Proceso, 
        DateTimeOffset? fechaInicioAsignacion, 
        DateTimeOffset? fechaTerminoAsignacion, 
        DateTimeOffset? fechaCreacion, 
        bool rolRequiereValidacion, 
        bool rolAsignadoValidado, 
        bool politicaAsignadaBase 
        )
    {
        Id = id;
        Id_Entidad = id_Entidad;
        Id_Rol = id_Rol;
        Id_Proceso = id_Proceso;
        FechaInicioAsignacion = fechaInicioAsignacion;
        FechaTerminoAsignacion = fechaTerminoAsignacion;
        FechaCreacion = fechaCreacion;
        RolRequiereValidacion = rolRequiereValidacion;
        RolAsignadoValidado = rolAsignadoValidado;
        PoliticaAsignadaBase = politicaAsignadaBase;
    }

    public Guid Id_Entidad { get; private set; } = Guid.Empty;
    public Guid Id_Rol { get; private set; } = Guid.Empty;
    public Guid Id_Proceso { get; private set; } = Guid.Empty;
    public DateTimeOffset? FechaInicioAsignacion { get; private set; }
    public DateTimeOffset? FechaTerminoAsignacion { get; private set; }
    public DateTimeOffset? FechaCreacion { get; private set; }
    public bool RolRequiereValidacion { get; private set; } = false;
    public bool RolAsignadoValidado { get; private set; } = false;
    public bool PoliticaAsignadaBase { get; private set; } = false;

    public void CambiarFechaInicioAsignacion(DateTimeOffset? nuevoValor)
    {
        FechaInicioAsignacion = nuevoValor;
    }

    public void CambiarFechaTerminoAsignacion(DateTimeOffset? nuevoValor)
    {
        FechaTerminoAsignacion = nuevoValor;
    }

    public void CambiarFechaCreacion(DateTimeOffset? nuevoValor)
    {
        FechaCreacion = nuevoValor;
    }

    public void CambiarRolRequiereValidacion(bool nuevoValor)
    {
        RolRequiereValidacion = nuevoValor;
    }

    public void CambiarRolAsignadoValidado(bool nuevoValor)
    {
        RolAsignadoValidado = nuevoValor;
    }

    public void CambiarPoliticaAsignadaBase(bool nuevoValor)
    {
        PoliticaAsignadaBase = nuevoValor;
    }
}

