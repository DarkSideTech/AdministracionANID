// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.145
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Organizaciones.Events;

namespace AUT2Services.Domain.Commands.Organizaciones.Handlers;

public partial class  OrganizacionCommandHandler :
    IRequestHandler<EliminarPor_CodigoOrganizacionCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarPor_CodigoOrganizacionCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existOrganizacion = await _organizacionRepository.BuscarPor_Codigo(command.Codigo);

        if (existOrganizacion is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

                    if (existOrganizacion.OrganizacionBase)
            {
                AddError($"Las Organizaciones marcadas como Base no se pueden eliminar");
                return CommandResponse;
            }
 
        existOrganizacion.AddDomainEvent(new OrganizacionEventEliminadoPor_Codigo(
            existOrganizacion.Id, 
            existOrganizacion.Codigo 
        ));

        _organizacionRepository.Eliminar(existOrganizacion);

        CommandResponse.Result = true;
        return await Commit(_organizacionRepository.UnitOfWork);
        }
}

