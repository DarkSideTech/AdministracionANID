// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.220
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

