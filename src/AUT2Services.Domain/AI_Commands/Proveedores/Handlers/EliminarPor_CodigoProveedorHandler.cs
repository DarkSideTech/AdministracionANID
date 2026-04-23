// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.121
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Events.Proveedores.Events;

namespace AUT2Services.Domain.Commands.Proveedores.Handlers;

public partial class  ProveedorCommandHandler :
    IRequestHandler<EliminarPor_CodigoProveedorCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(EliminarPor_CodigoProveedorCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid()) return CommandResponse;

        var existProveedor = await _proveedorRepository.BuscarPor_Codigo(command.Codigo);

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
 
        AddDeleteDomainEvent(command, existProveedor, new ProveedorEventEliminadoPor_Codigo(
            existProveedor.Id, 
            existProveedor.Codigo 
        ), existProveedor);

        _proveedorRepository.Eliminar(existProveedor);

        CommandResponse.Result = true;
        return await Commit(_proveedorRepository.UnitOfWork);
        }
}

