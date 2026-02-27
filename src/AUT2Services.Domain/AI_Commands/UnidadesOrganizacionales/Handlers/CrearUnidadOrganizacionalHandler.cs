// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.262
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Handlers;

public partial class UnidadOrganizacionalCommandHandler :
    IRequestHandler<CrearUnidadOrganizacionalCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearUnidadOrganizacionalCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existUnidadOrganizacional = await _unidadOrganizacionalRepository.BuscarPor_Codigo_Id_Organizacion(command.Codigo, command.Id_Organizacion);

        if(existUnidadOrganizacional is not null)
        {
            AddError($"Ya existe UnidadOrganizacional para la busqueda : Codigo [{command.Codigo}] Id_Organizacion [{command.Id_Organizacion}] ");
            return CommandResponse;
        }
        
        var newUnidadOrganizacional = new UnidadOrganizacional(
            Guid.NewGuid(), 
            command.Id_Organizacion, 
            command.Codigo, 
            command.Nombre, 
            command.Descripcion, 
            command.UnidadOrganizacionalBase, 
            command.Activo 
        );

                newUnidadOrganizacional.CambiarUnidadOrganizacionalBase(false);
        newUnidadOrganizacional.CambiarActivo(true);

        newUnidadOrganizacional.AddDomainEvent(new UnidadOrganizacionalEventCreado(
            newUnidadOrganizacional.Id, 
        newUnidadOrganizacional.Id_Organizacion, 
        newUnidadOrganizacional.Codigo, 
        newUnidadOrganizacional.Nombre, 
        newUnidadOrganizacional.Descripcion, 
        newUnidadOrganizacional.UnidadOrganizacionalBase, 
        newUnidadOrganizacional.Activo 
            )
        );

        _unidadOrganizacionalRepository.Crear(newUnidadOrganizacional);

        CommandResponse.Data = JsonConvert.SerializeObject(new UnidadOrganizacionalDTO(){
            Id = newUnidadOrganizacional.Id, 
            Id_Organizacion = newUnidadOrganizacional.Id_Organizacion, 
            Codigo = newUnidadOrganizacional.Codigo, 
            Nombre = newUnidadOrganizacional.Nombre, 
            Descripcion = newUnidadOrganizacional.Descripcion, 
            UnidadOrganizacionalBase = newUnidadOrganizacional.UnidadOrganizacionalBase, 
            Activo = newUnidadOrganizacional.Activo 
        });

        CommandResponse.Result = true;
        return await Commit(_unidadOrganizacionalRepository.UnitOfWork);
        }
}

