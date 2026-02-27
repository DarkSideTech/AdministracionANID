// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.231
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Organizaciones.Events;

public class OrganizacionEventDesactivado : Event
{
    public OrganizacionEventDesactivado(
        Guid id, 
            bool activo 
        )
    {
        Id = id;
        Activo = activo; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public bool Activo  { get; private set; } = false; 
}

