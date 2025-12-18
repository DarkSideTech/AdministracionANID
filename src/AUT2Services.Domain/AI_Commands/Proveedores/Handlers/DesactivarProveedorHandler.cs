// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.095
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.Proveedores.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.Proveedores.Handlers
{
    public partial class ProveedorCommandHandler :
        IRequestHandler<DesactivarProveedorCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(DesactivarProveedorCommand command, CancellationToken cancellationToken)
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

        if (!existProveedor.Activo)
        {
            AddError($"El Porveedor ya se encuentra desactivado");
            return CommandResponse;
        }

        newProveedor.CambiarActivo(false);

            newProveedor.AddDomainEvent(new ProveedorEventDesactivado(
                    newProveedor.Id, 
                    newProveedor.Activo 
                    )
                );

            _proveedorRepository.Modificar(newProveedor);
        
            CommandResponse.Data = JsonConvert.SerializeObject(new ProveedorDTO(){
                    Id = newProveedor.Id, 
                    Codigo = newProveedor.Codigo, 
                    Nombre = newProveedor.Nombre, 
                    Descripcion = newProveedor.Descripcion, 
                    APIDeAutenticacion = newProveedor.APIDeAutenticacion, 
                    ProveedorBase = newProveedor.ProveedorBase, 
                    Activo = newProveedor.Activo 
                });
            CommandResponse.Result = true;

            return await Commit(_proveedorRepository.UnitOfWork);
            }
    }
}

