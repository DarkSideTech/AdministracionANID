// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.268
// -------------------------------------------------
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Entidades.Events;

namespace AUT2Services.Domain.Commands.Entidades.Handlers;

public partial class  EntidadCommandHandler :
    IRequestHandler<EliminarEntidadCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarEntidadCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existEntidad = await _entidadRepository.BuscarPor_Id(command.Id);

        if (existEntidad is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

                    if (existEntidad.EntidadBase)
            {
                AddError($"Los entidades marcados como Base no se pueden eliminar");
                return CommandResponse;
            }
 
        existEntidad.AddDomainEvent(new EntidadEventEliminado(
            existEntidad.Id 
        ));

        _entidadRepository.Eliminar(existEntidad);

        CommandResponse.Result = true;
        return await Commit(_entidadRepository.UnitOfWork);
        }
}

