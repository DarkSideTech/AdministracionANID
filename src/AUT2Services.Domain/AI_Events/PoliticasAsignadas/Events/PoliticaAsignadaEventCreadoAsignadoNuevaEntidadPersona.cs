// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.104
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.PoliticasAsignadas.Events;

public class PoliticaAsignadaEventCreadoAsignadoNuevaEntidadPersona : Event
{
    public PoliticaAsignadaEventCreadoAsignadoNuevaEntidadPersona(
        Guid id, 
            Guid id_Entidad, 
            Guid id_Rol, 
            Guid id_Proceso, 
            DateTimeOffset fechaInicioAsignacion, 
            DateTimeOffset fechaTerminoAsignacion, 
            DateTimeOffset fechaCreacion, 
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

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_Entidad  { get; private set; } = Guid.Empty; 
    public Guid Id_Rol  { get; private set; } = Guid.Empty; 
    public Guid Id_Proceso  { get; private set; } = Guid.Empty; 
    public DateTimeOffset FechaInicioAsignacion  { get; private set; } = DateTimeOffset.MinValue; 
    public DateTimeOffset FechaTerminoAsignacion  { get; private set; } = DateTimeOffset.MinValue; 
    public DateTimeOffset FechaCreacion  { get; private set; } = DateTimeOffset.MinValue; 
    public bool RolRequiereValidacion  { get; private set; } = false; 
    public bool RolAsignadoValidado  { get; private set; } = true; 
    public bool PoliticaAsignadaBase  { get; private set; } = false; 
}

