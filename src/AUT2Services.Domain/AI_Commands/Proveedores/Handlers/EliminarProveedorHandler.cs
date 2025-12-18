// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.094
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Proveedores.Events;

namespace AUT2Services.Domain.Commands.Proveedores.Handlers;

public partial class  ProveedorCommandHandler :
    IRequestHandler<EliminarProveedorCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarProveedorCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existProveedor = await _proveedorRepository.BuscarPor_Id(command.Id);

        if (existProveedor is null)
        {
            AddError($"El elemnto buscado no existe, no es posible eliminarlo");
            return CommandResponse;
        }

                if (existProveedor.ProveedorBase)
        {
            AddError($"Los Proveedores marcados como Base no se pueden eliminar");
            return CommandResponse;
        }
 
        existProveedor.AddDomainEvent(new ProveedorEventEliminado(
            existProveedor.Id 
        ));

        _proveedorRepository.Eliminar(existProveedor);

        CommandResponse.Result = true;
        return await Commit(_proveedorRepository.UnitOfWork);
        }
}

