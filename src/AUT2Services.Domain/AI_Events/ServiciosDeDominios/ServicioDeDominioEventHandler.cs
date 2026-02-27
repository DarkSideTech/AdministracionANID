// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.234
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.ServiciosDeDominios.Events;

namespace AUT2Services.Domain.Events.ServiciosDeDominios;

public class ServicioDeDominioEventHandler :
    INotificationHandler<ServicioDeDominioEventEnrrolamientoValidado>, 
    INotificationHandler<ServicioDeDominioEventAsignacionDeRolValidada>, 
    INotificationHandler<ServicioDeDominioEventEntidadNuevaCreada>, 
    INotificationHandler<ServicioDeDominioEventEntidadMarcadaComoPrincipal> 
{
    public Task Handle(ServicioDeDominioEventEnrrolamientoValidado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ServicioDeDominioEventAsignacionDeRolValidada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ServicioDeDominioEventEntidadNuevaCreada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(ServicioDeDominioEventEntidadMarcadaComoPrincipal eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

