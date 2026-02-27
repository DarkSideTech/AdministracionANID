// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.273
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.Organizaciones.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.Organizaciones.Handlers;

public partial class OrganizacionCommandHandler :
    IRequestHandler<CrearOrganizacionCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearOrganizacionCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existOrganizacion = await _organizacionRepository.BuscarPor_Codigo(command.Codigo);

        if(existOrganizacion is not null)
        {
            AddError($"Ya existe Organizacion para la busqueda : Codigo [{command.Codigo}] ");
            return CommandResponse;
        }
        
        var newOrganizacion = new Organizacion(
            Guid.NewGuid(), 
            command.IdOrganizacion, 
            command.Codigo, 
            command.Nombre, 
            command.Descripcion, 
            command.OrganizacionBase, 
            command.Activo 
        );

                newOrganizacion.CambiarOrganizacionBase(false);
        newOrganizacion.CambiarActivo(true);

        newOrganizacion.AddDomainEvent(new OrganizacionEventCreado(
            newOrganizacion.Id, 
        newOrganizacion.IdOrganizacion, 
        newOrganizacion.Codigo, 
        newOrganizacion.Nombre, 
        newOrganizacion.Descripcion, 
        newOrganizacion.OrganizacionBase, 
        newOrganizacion.Activo 
            )
        );

        _organizacionRepository.Crear(newOrganizacion);

        CommandResponse.Data = JsonConvert.SerializeObject(new OrganizacionDTO(){
            Id = newOrganizacion.Id, 
            IdOrganizacion = newOrganizacion.IdOrganizacion, 
            Codigo = newOrganizacion.Codigo, 
            Nombre = newOrganizacion.Nombre, 
            Descripcion = newOrganizacion.Descripcion, 
            OrganizacionBase = newOrganizacion.OrganizacionBase, 
            Activo = newOrganizacion.Activo 
        });

        CommandResponse.Result = true;
        return await Commit(_organizacionRepository.UnitOfWork);
        }
}

