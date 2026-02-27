// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.258
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Handlers;

public partial class  ValidacionEnrrolamientoCommandHandler :
    IRequestHandler<EliminarValidacionEnrrolamientoCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarValidacionEnrrolamientoCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existValidacionEnrrolamiento = await _validacionEnrrolamientoRepository.BuscarPor_Id(command.Id);

        if (existValidacionEnrrolamiento is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

         
        existValidacionEnrrolamiento.AddDomainEvent(new ValidacionEnrrolamientoEventEliminado(
            existValidacionEnrrolamiento.Id 
        ));

        _validacionEnrrolamientoRepository.Eliminar(existValidacionEnrrolamiento);

        CommandResponse.Result = true;
        return await Commit(_validacionEnrrolamientoRepository.UnitOfWork);
        }
}

