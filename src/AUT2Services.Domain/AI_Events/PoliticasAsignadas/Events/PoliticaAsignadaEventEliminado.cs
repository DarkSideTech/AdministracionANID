// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.104
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.PoliticasAsignadas.Events;

public class PoliticaAsignadaEventEliminado : Event
{
    public PoliticaAsignadaEventEliminado(
        Guid id,
        Guid? auditAggregateId = null
        )
    {
        Id = id;

        AggregateId = auditAggregateId ?? id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
}

