// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.284
// -------------------------------------------------
using AUT2Services.Domain.Commands.Procesos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.Procesos.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.Procesos.Handlers;

public partial class ProcesoCommandHandler :
    IRequestHandler<CrearProcesoCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearProcesoCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existProceso = await _procesoRepository.BuscarPor_Codigo(command.Codigo);

        if(existProceso is not null)
        {
            AddError($"Ya existe Proceso para la busqueda : Codigo [{command.Codigo}] ");
            return CommandResponse;
        }
        
        var newProceso = new Proceso(
            Guid.NewGuid(), 
            command.IdMacro_Proceso, 
            command.Codigo, 
            command.Nombre, 
            command.Descripcion, 
            command.Contexto, 
            command.NivelDeProceso, 
            command.Url, 
            command.Token, 
            command.ComoDesplegarUrlDeProceso, 
            command.ProcesoBase, 
            command.MaximaAsignacionDeRoles, 
            command.Activo 
        );

                newProceso.CambiarProcesoBase(false);
        newProceso.CambiarMaximaAsignacionDeRoles(1);
        newProceso.CambiarActivo(true);

        newProceso.AddDomainEvent(new ProcesoEventCreado(
            newProceso.Id, 
        newProceso.IdMacro_Proceso, 
        newProceso.Codigo, 
        newProceso.Nombre, 
        newProceso.Descripcion, 
        newProceso.Contexto, 
        newProceso.NivelDeProceso, 
        newProceso.Url, 
        newProceso.Token, 
        newProceso.ComoDesplegarUrlDeProceso, 
        newProceso.ProcesoBase, 
        newProceso.MaximaAsignacionDeRoles, 
        newProceso.Activo 
            )
        );

        _procesoRepository.Crear(newProceso);

        CommandResponse.Data = JsonConvert.SerializeObject(new ProcesoDTO(){
            Id = newProceso.Id, 
            IdMacro_Proceso = newProceso.IdMacro_Proceso, 
            Codigo = newProceso.Codigo, 
            Nombre = newProceso.Nombre, 
            Descripcion = newProceso.Descripcion, 
            Contexto = newProceso.Contexto, 
            NivelDeProceso = newProceso.NivelDeProceso, 
            Url = newProceso.Url, 
            Token = newProceso.Token, 
            ComoDesplegarUrlDeProceso = newProceso.ComoDesplegarUrlDeProceso, 
            ProcesoBase = newProceso.ProcesoBase, 
            MaximaAsignacionDeRoles = newProceso.MaximaAsignacionDeRoles, 
            Activo = newProceso.Activo 
        });

        CommandResponse.Result = true;
        return await Commit(_procesoRepository.UnitOfWork);
        }
}

