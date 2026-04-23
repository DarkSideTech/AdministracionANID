// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.126
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.AutenticadoresExternos.Events;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Handlers;

public partial class  AutenticadorExternoCommandHandler :
    IRequestHandler<EliminarAutenticadorExternoCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarAutenticadorExternoCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existAutenticadorExterno = await _autenticadorExternoRepository.BuscarPor_Id(command.Id);

        if (existAutenticadorExterno is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

            if (existAutenticadorExterno.ValidadorPrimario)
        {
            AddError($"No es posible eliminar el Validador Primario, primero debe eliminar la marca de primario");
            return CommandResponse;
        }

        if (existAutenticadorExterno.AutenticadorExternoBase)
        {
            AddError($"No es posible modificar un autenticador externo marcado como base");
            return CommandResponse;
        }
 
        AddDeleteDomainEvent(command, existAutenticadorExterno, new AutenticadorExternoEventEliminado(
            existAutenticadorExterno.Id
        ), existAutenticadorExterno);

        _autenticadorExternoRepository.Eliminar(existAutenticadorExterno);

        CommandResponse.Result = true;
        return await Commit(_autenticadorExternoRepository.UnitOfWork);
        }
}

