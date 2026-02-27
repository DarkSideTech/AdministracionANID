// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.223
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.AutenticadoresExternos.Events;

public class AutenticadorExternoEventEliminado : Event
{
    public AutenticadorExternoEventEliminado(
        Guid id 
        )
    {
        Id = id;

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
}

