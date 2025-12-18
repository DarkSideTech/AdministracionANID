// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.084
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;

namespace AUT2Services.Domain.Events.UnidadesOrganizacionales;

public class UnidadOrganizacionalEventHandler :
    INotificationHandler<UnidadOrganizacionalEventCreado>, 
    INotificationHandler<UnidadOrganizacionalEventModificado>, 
    INotificationHandler<UnidadOrganizacionalEventEliminado>, 
    INotificationHandler<UnidadOrganizacionalEventEliminadoPor_Codigo_Id_Organizacion>, 
    INotificationHandler<UnidadOrganizacionalEventActivado>, 
    INotificationHandler<UnidadOrganizacionalEventDesactivado> 
{
    public Task Handle(UnidadOrganizacionalEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(UnidadOrganizacionalEventModificado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(UnidadOrganizacionalEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(UnidadOrganizacionalEventEliminadoPor_Codigo_Id_Organizacion eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(UnidadOrganizacionalEventActivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(UnidadOrganizacionalEventDesactivado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

