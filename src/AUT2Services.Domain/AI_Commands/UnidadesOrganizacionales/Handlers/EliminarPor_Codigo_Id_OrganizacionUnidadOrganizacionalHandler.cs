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
    IRequestHandler<EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existUnidadOrganizacional = await _unidadOrganizacionalRepository.BuscarPor_Codigo_Id_Organizacion(command.Codigo, command.Id_Organizacion);

        if (existUnidadOrganizacional is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

         
        existUnidadOrganizacional.AddDomainEvent(new UnidadOrganizacionalEventEliminadoPor_Codigo_Id_Organizacion(
            existUnidadOrganizacional.Id, 
            existUnidadOrganizacional.Codigo, 
            existUnidadOrganizacional.Id_Organizacion 
        ));

        _unidadOrganizacionalRepository.Eliminar(existUnidadOrganizacional);

        CommandResponse.Result = true;
        return await Commit(_unidadOrganizacionalRepository.UnitOfWork);
        }
}

