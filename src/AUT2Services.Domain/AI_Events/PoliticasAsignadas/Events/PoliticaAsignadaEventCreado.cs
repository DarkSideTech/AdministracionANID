// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.103
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.PoliticasAsignadas.Events;

public class PoliticaAsignadaEventCreado : Event
{
    public PoliticaAsignadaEventCreado(
        Guid id, 
            Guid id_Entidad, 
            Guid id_Rol, 
            Guid id_Proceso, 
            bool rolRequiereValidacion, 
            DateTimeOffset fechaInicioAsignacion, 
            DateTimeOffset fechaCreacion, 
            bool politicaAsignadaBase 
        )
    {
        Id = id;
        Id_Entidad = id_Entidad; 
        Id_Rol = id_Rol; 
        Id_Proceso = id_Proceso; 
        RolRequiereValidacion = rolRequiereValidacion; 
        FechaInicioAsignacion = fechaInicioAsignacion; 
        FechaCreacion = fechaCreacion; 
        PoliticaAsignadaBase = politicaAsignadaBase; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_Entidad  { get; private set; } = Guid.Empty; 
    public Guid Id_Rol  { get; private set; } = Guid.Empty; 
    public Guid Id_Proceso  { get; private set; } = Guid.Empty; 
    public bool RolRequiereValidacion  { get; private set; } = false; 
    public DateTimeOffset FechaInicioAsignacion  { get; private set; } = DateTimeOffset.MinValue; 
    public DateTimeOffset FechaCreacion  { get; private set; } = DateTimeOffset.MinValue; 
    public bool PoliticaAsignadaBase  { get; private set; } = false; 
}

