// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.151
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.PoliticasAsignadas.Events;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;

public partial class  PoliticaAsignadaCommandHandler :
    IRequestHandler<EliminarPoliticaAsignadaCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarPoliticaAsignadaCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existPoliticaAsignada = await _politicaAsignadaRepository.BuscarPor_Id(command.Id);

        if (existPoliticaAsignada is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

                if (existPoliticaAsignada.PoliticaAsignadaBase)
            {
                AddError($"Las Politicas Asignadas marcadas como Base no se pueden eliminar");
                return CommandResponse;
            }
 
        AddDeleteDomainEvent(command, existPoliticaAsignada, new PoliticaAsignadaEventEliminado(
            existPoliticaAsignada.Id,
            command.AuditAggregateId
        ), existPoliticaAsignada);

        _politicaAsignadaRepository.Eliminar(existPoliticaAsignada);

        CommandResponse.Result = true;
        return await Commit(_politicaAsignadaRepository.UnitOfWork);
        }
}

