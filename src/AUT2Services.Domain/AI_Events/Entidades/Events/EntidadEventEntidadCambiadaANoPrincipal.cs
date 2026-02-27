// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.229
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Entidades.Events;

public class EntidadEventEntidadCambiadaANoPrincipal : Event
{
    public EntidadEventEntidadCambiadaANoPrincipal(
        Guid id, 
            bool principal 
        )
    {
        Id = id;
        Principal = principal; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public bool Principal  { get; private set; } = false; 
}

