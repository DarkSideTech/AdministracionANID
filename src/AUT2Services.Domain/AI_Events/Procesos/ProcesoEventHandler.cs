// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.105
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Procesos.Events;

namespace AUT2Services.Domain.Events.Procesos;

public class ProcesoEventHandler :
    INotificationHandler<ProcesoEventCreado>, 
    INotificationHandler<ProcesoEventModificado>, 
    INotificationHandler<ProcesoEventEliminado>, 
    INotificationHandler<ProcesoEventActivada>, 
    INotificationHandler<ProcesoEventDesactivada> 
{
    public Task Handle(ProcesoEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProcesoEventModificado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProcesoEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProcesoEventActivada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ProcesoEventDesactivada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

