// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.090
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

