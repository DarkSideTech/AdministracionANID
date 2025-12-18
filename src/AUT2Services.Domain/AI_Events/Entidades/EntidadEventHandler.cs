// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.084
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Entidades.Events;

namespace AUT2Services.Domain.Events.Entidades;

public class EntidadEventHandler :
    INotificationHandler<EntidadEventCreado>, 
    INotificationHandler<EntidadEventModificado>, 
    INotificationHandler<EntidadEventEliminado>, 
    INotificationHandler<EntidadEventAutorizacionFinalizada>, 
    INotificationHandler<EntidadEventEntidadCambiadaAPrincipal>, 
    INotificationHandler<EntidadEventEntidadCambiadaANoPrincipal> 
{
    public Task Handle(EntidadEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(EntidadEventModificado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(EntidadEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(EntidadEventAutorizacionFinalizada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(EntidadEventEntidadCambiadaAPrincipal eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(EntidadEventEntidadCambiadaANoPrincipal eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

