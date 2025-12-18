// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.118
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.PoliticasAsignadas.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;

public partial class PoliticaAsignadaCommandHandler :
    IRequestHandler<CrearPoliticaAsignadaCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearPoliticaAsignadaCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existPoliticaAsignada = await _politicaAsignadaRepository.BuscarPor_Id_Entidad_Id_Rol_Id_Proceso(command.Id_Entidad, command.Id_Rol, command.Id_Proceso);

        if(existPoliticaAsignada is not null)
        {
            AddError($"Ya existe PoliticaAsignada para la busqueda : Id_Entidad [{command.Id_Entidad}] Id_Rol [{command.Id_Rol}] Id_Proceso [{command.Id_Proceso}] ");
            return CommandResponse;
        }
        
        var newPoliticaAsignada = new PoliticaAsignada(
            Guid.NewGuid(), 
            command.Id_Entidad, 
            command.Id_Rol, 
            command.Id_Proceso, 
            command.FechaInicioAsignacion, 
            command.FechaTerminoAsignacion, 
            command.FechaCreacion, 
            command.RolRequiereValidacion, 
            command.RolAsignadoValidado, 
            command.PoliticaAsignadaBase 
        );

            newPoliticaAsignada.CambiarRolAsignadoValidado(!newPoliticaAsignada.RolRequiereValidacion);

        newPoliticaAsignada.AddDomainEvent(new PoliticaAsignadaEventCreado(
            newPoliticaAsignada.Id, 
        newPoliticaAsignada.Id_Entidad, 
        newPoliticaAsignada.Id_Rol, 
        newPoliticaAsignada.Id_Proceso, 
        newPoliticaAsignada.RolRequiereValidacion, 
        newPoliticaAsignada.FechaInicioAsignacion, 
        newPoliticaAsignada.FechaCreacion, 
        newPoliticaAsignada.PoliticaAsignadaBase 
            )
        );

        _politicaAsignadaRepository.Crear(newPoliticaAsignada);

        CommandResponse.Data = JsonConvert.SerializeObject(new PoliticaAsignadaDTO(){
            Id = newPoliticaAsignada.Id, 
            Id_Entidad = newPoliticaAsignada.Id_Entidad, 
            Id_Rol = newPoliticaAsignada.Id_Rol, 
            Id_Proceso = newPoliticaAsignada.Id_Proceso, 
            FechaInicioAsignacion = newPoliticaAsignada.FechaInicioAsignacion, 
            FechaTerminoAsignacion = newPoliticaAsignada.FechaTerminoAsignacion, 
            FechaCreacion = newPoliticaAsignada.FechaCreacion, 
            RolRequiereValidacion = newPoliticaAsignada.RolRequiereValidacion, 
            RolAsignadoValidado = newPoliticaAsignada.RolAsignadoValidado, 
            PoliticaAsignadaBase = newPoliticaAsignada.PoliticaAsignadaBase 
        });

        CommandResponse.Result = true;
        return await Commit(_politicaAsignadaRepository.UnitOfWork);
        }
}

