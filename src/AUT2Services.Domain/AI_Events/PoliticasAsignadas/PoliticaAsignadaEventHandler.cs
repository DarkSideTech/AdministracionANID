// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.103
// -------------------------------------------------
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.PoliticasAsignadas.Events;

namespace AUT2Services.Domain.Events.PoliticasAsignadas;

public class PoliticaAsignadaEventHandler :
    INotificationHandler<PoliticaAsignadaEventCreado>, 
    INotificationHandler<PoliticaAsignadaEventCreadoAsignadoNuevaEntidadPersona>, 
    INotificationHandler<PoliticaAsignadaEventEliminado>, 
    INotificationHandler<PoliticaAsignadaEventAsignacionFinalizada>, 
    INotificationHandler<PoliticaAsignadaEventAsignacionDeRolValidada> 
{
    public Task Handle(PoliticaAsignadaEventCreado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(PoliticaAsignadaEventCreadoAsignadoNuevaEntidadPersona eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(PoliticaAsignadaEventEliminado eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(PoliticaAsignadaEventAsignacionFinalizada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    public Task Handle(PoliticaAsignadaEventAsignacionDeRolValidada eventData, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
} 

