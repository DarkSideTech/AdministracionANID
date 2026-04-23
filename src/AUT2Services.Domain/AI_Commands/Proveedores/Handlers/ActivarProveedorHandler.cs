// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.121
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.Proveedores.Events;
using System.Text.Json;

namespace AUT2Services.Domain.Commands.Proveedores.Handlers
{
    public partial class ProveedorCommandHandler :
        IRequestHandler<ActivarProveedorCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ActivarProveedorCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existProveedor = await _proveedorRepository.BuscarPor_Id(command.Id);

            if (existProveedor is null)
            {
                AddError($"El Id de Proveedor: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newProveedor = new Proveedor(
                existProveedor.Id, 
                existProveedor.Codigo, 
                existProveedor.Nombre, 
                existProveedor.Descripcion, 
                existProveedor.APIDeAutenticacion, 
                existProveedor.ProveedorBase, 
                existProveedor.Activo 
                );

                if (existProveedor.ProveedorBase)
        {
            AddError($"Los Proveedores marcados como Base no se pueden modificar");
            return CommandResponse;
        }

        if (existProveedor.Activo)
        {
            AddError($"El Porveedor ya se encuentra activa");
            return CommandResponse;
        }

        newProveedor.CambiarActivo(true);

            AddUpdateDomainEvent(command, newProveedor, new ProveedorEventActivado(
                    newProveedor.Id, 
                    newProveedor.Activo 
                    ), existProveedor, newProveedor);

            _proveedorRepository.Modificar(newProveedor);
        
            CommandResponse.Data = new ProveedorDTO(){
                    Id = newProveedor.Id, 
                    Codigo = newProveedor.Codigo, 
                    Nombre = newProveedor.Nombre, 
                    Descripcion = newProveedor.Descripcion, 
                    APIDeAutenticacion = newProveedor.APIDeAutenticacion, 
                    ProveedorBase = newProveedor.ProveedorBase, 
                    Activo = newProveedor.Activo 
                };
            CommandResponse.Result = true;

            return await Commit(_proveedorRepository.UnitOfWork);
            }
    }
}

