// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.224
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;

namespace AUT2Services.Domain.Events.ValidacionEnrrolamientos;

public class ValidacionEnrrolamientoEventHandler :
    INotificationHandler<ValidacionEnrrolamientoEventCreado>, 
    INotificationHandler<ValidacionEnrrolamientoEventEliminado>, 
    INotificationHandler<ValidacionEnrrolamientoEventActivado>, 
    INotificationHandler<ValidacionEnrrolamientoEventDesactivado> 
{
    public Task Handle(ValidacionEnrrolamientoEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ValidacionEnrrolamientoEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ValidacionEnrrolamientoEventActivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ValidacionEnrrolamientoEventDesactivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

