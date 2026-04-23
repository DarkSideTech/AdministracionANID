// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.098
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;

namespace AUT2Services.Domain.Events.Entidades.Events;

public class EntidadEventAutorizacionFinalizada : Event
{
    public EntidadEventAutorizacionFinalizada(
        Guid id, 
        DateTimeOffset? fechaTerminoAutorizacion 
        )
    {
        Id = id;
        FechaTerminoAutorizacion = fechaTerminoAutorizacion; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public DateTimeOffset? FechaTerminoAutorizacion  { get; private set; }
}

