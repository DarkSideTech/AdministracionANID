// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.082
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.AutenticadoresExternos.Events;

namespace AUT2Services.Domain.Events.AutenticadoresExternos;

public class AutenticadorExternoEventHandler :
    INotificationHandler<AutenticadorExternoEventCreado>, 
    INotificationHandler<AutenticadorExternoEventModificado>, 
    INotificationHandler<AutenticadorExternoEventEliminado>, 
    INotificationHandler<AutenticadorExternoEventValidadorPrimarioMarcado>, 
    INotificationHandler<AutenticadorExternoEventActivado>, 
    INotificationHandler<AutenticadorExternoEventDesactivado> 
{
    public Task Handle(AutenticadorExternoEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(AutenticadorExternoEventModificado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(AutenticadorExternoEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(AutenticadorExternoEventValidadorPrimarioMarcado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(AutenticadorExternoEventActivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(AutenticadorExternoEventDesactivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

