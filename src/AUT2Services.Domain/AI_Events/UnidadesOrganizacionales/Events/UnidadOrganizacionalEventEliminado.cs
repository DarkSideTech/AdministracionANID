// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.084
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;

public class UnidadOrganizacionalEventEliminado : Event
{
    public UnidadOrganizacionalEventEliminado(
        Guid id 
        )
    {
        Id = id;

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
}

