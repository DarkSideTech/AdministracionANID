// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.278
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.PoliticasAsignadas.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;

public partial class PoliticaAsignadaCommandHandler :
    IRequestHandler<CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand command, CancellationToken cancellationToken)
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

                newPoliticaAsignada.CambiarFechaInicioAsignacion(DateTimeOffset.Now);
        newPoliticaAsignada.CambiarFechaTerminoAsignacion(DateTimeOffset.MinValue);
        newPoliticaAsignada.CambiarFechaCreacion(DateTimeOffset.Now);
        newPoliticaAsignada.CambiarRolRequiereValidacion(false);
        newPoliticaAsignada.CambiarRolAsignadoValidado(true);
        newPoliticaAsignada.CambiarPoliticaAsignadaBase(false);

        newPoliticaAsignada.AddDomainEvent(new PoliticaAsignadaEventCreadoAsignadoNuevaEntidadPersona(
            newPoliticaAsignada.Id, 
        newPoliticaAsignada.Id_Entidad, 
        newPoliticaAsignada.Id_Rol, 
        newPoliticaAsignada.Id_Proceso, 
        newPoliticaAsignada.FechaInicioAsignacion, 
        newPoliticaAsignada.FechaTerminoAsignacion, 
        newPoliticaAsignada.FechaCreacion, 
        newPoliticaAsignada.RolRequiereValidacion, 
        newPoliticaAsignada.RolAsignadoValidado, 
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

