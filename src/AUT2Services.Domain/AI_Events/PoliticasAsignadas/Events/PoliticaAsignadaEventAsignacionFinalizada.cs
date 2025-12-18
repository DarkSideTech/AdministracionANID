// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.088
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.PoliticasAsignadas.Events;

public class PoliticaAsignadaEventAsignacionFinalizada : Event
{
    public PoliticaAsignadaEventAsignacionFinalizada(
        Guid id, 
            DateTimeOffset fechaTerminoAsignacion 
        )
    {
        Id = id;
        FechaTerminoAsignacion = fechaTerminoAsignacion; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public DateTimeOffset FechaTerminoAsignacion  { get; private set; } = DateTimeOffset.MinValue; 
}

