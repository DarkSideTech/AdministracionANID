// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.086
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Organizaciones.Events;

namespace AUT2Services.Domain.Events.Organizaciones;

public class OrganizacionEventHandler :
    INotificationHandler<OrganizacionEventCreado>, 
    INotificationHandler<OrganizacionEventModificado>, 
    INotificationHandler<OrganizacionEventEliminado>, 
    INotificationHandler<OrganizacionEventEliminadoPor_Codigo>, 
    INotificationHandler<OrganizacionEventActivado>, 
    INotificationHandler<OrganizacionEventDesactivado> 
{
    public Task Handle(OrganizacionEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(OrganizacionEventModificado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(OrganizacionEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(OrganizacionEventEliminadoPor_Codigo eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(OrganizacionEventActivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(OrganizacionEventDesactivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

