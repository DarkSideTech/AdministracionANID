// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.116
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.Proveedores.Events;
using System.Text.Json;

namespace AUT2Services.Domain.Commands.Proveedores.Handlers;

public partial class ProveedorCommandHandler :
    IRequestHandler<CrearProveedorCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearProveedorCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existProveedor = await _proveedorRepository.BuscarPor_Codigo(command.Codigo);

        if(existProveedor is not null)
        {
            AddError($"Ya existe Proveedor para la busqueda : Codigo [{command.Codigo}] ");
            return CommandResponse;
        }
        
        var newProveedor = new Proveedor(
            Guid.NewGuid(), 
            command.Codigo, 
            command.Nombre, 
            command.Descripcion, 
            command.APIDeAutenticacion, 
            false, 
            command.Activo 
        );

                newProveedor.CambiarActivo(true);

        AddCreateDomainEvent(command, newProveedor, new ProveedorEventCreado(
            newProveedor.Id, 
        newProveedor.Codigo, 
        newProveedor.Nombre, 
        newProveedor.Descripcion, 
        newProveedor.APIDeAutenticacion 
            )
        , newProveedor);

        _proveedorRepository.Crear(newProveedor);

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

