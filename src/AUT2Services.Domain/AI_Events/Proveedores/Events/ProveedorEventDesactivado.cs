// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.082
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Proveedores.Events;

public class ProveedorEventDesactivado : Event
{
    public ProveedorEventDesactivado(
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

