// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.081
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Proveedores.Events;

namespace AUT2Services.Domain.Events.Proveedores;

public class ProveedorEventHandler :
    INotificationHandler<ProveedorEventCreado>, 
    INotificationHandler<ProveedorEventModificado>, 
    INotificationHandler<ProveedorEventEliminado>, 
    INotificationHandler<ProveedorEventEliminadoPor_Codigo>, 
    INotificationHandler<ProveedorEventActivado>, 
    INotificationHandler<ProveedorEventDesactivado> 
{
    public Task Handle(ProveedorEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProveedorEventModificado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProveedorEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProveedorEventEliminadoPor_Codigo eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProveedorEventActivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProveedorEventDesactivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

