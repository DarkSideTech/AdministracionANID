// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.134
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Handlers;

public partial class  UnidadOrganizacionalCommandHandler :
    IRequestHandler<EliminarUnidadOrganizacionalCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarUnidadOrganizacionalCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existUnidadOrganizacional = await _unidadOrganizacionalRepository.BuscarPor_Id(command.Id);

        if (existUnidadOrganizacional is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

                if (existUnidadOrganizacional.UnidadOrganizacionalBase)
        {
            AddError($"Las unidades organizacionales marcadas como Base no se pueden eliminar");
            return CommandResponse;
        }
 
        existUnidadOrganizacional.AddDomainEvent(new UnidadOrganizacionalEventEliminado(
            existUnidadOrganizacional.Id 
        ));

        _unidadOrganizacionalRepository.Eliminar(existUnidadOrganizacional);

        CommandResponse.Result = true;
        return await Commit(_unidadOrganizacionalRepository.UnitOfWork);
        }
}

