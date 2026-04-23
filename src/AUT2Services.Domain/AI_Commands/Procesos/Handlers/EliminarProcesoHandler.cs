// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.156
// -------------------------------------------------
using AUT2Services.Domain.Commands.Procesos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Procesos.Events;

namespace AUT2Services.Domain.Commands.Procesos.Handlers;

public partial class  ProcesoCommandHandler :
    IRequestHandler<EliminarProcesoCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarProcesoCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existProceso = await _procesoRepository.BuscarPor_Id(command.Id);

        if (existProceso is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

                if (existProceso.ProcesoBase)
        {
            AddError($"Los Procesos marcadas como Base no se pueden eliminar");
            return CommandResponse;
        }
 
        AddDeleteDomainEvent(command, existProceso, new ProcesoEventEliminado(
            existProceso.Id 
        ), existProceso);

        _procesoRepository.Eliminar(existProceso);

        CommandResponse.Result = true;
        return await Commit(_procesoRepository.UnitOfWork);
        }
}

