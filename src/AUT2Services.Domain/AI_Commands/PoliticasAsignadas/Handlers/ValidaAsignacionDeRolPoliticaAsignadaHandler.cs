// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.151
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.PoliticasAsignadas.Events;
using System.Text.Json;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers
{
    public partial class PoliticaAsignadaCommandHandler :
        IRequestHandler<ValidaAsignacionDeRolPoliticaAsignadaCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ValidaAsignacionDeRolPoliticaAsignadaCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existPoliticaAsignada = await _politicaAsignadaRepository.BuscarPor_Id(command.Id);

            if (existPoliticaAsignada is null)
            {
                AddError($"El Id de PoliticaAsignada: [{command.Id}], no Existe!");
                return CommandResponse;
            }

            if (existPoliticaAsignada.PoliticaAsignadaBase)
            {
                AddError($"Las Politicas Asignadas marcadas como Base no se pueden modificar");
                return CommandResponse;
            }
         
            var newPoliticaAsignada = new PoliticaAsignada(
                existPoliticaAsignada.Id, 
                existPoliticaAsignada.Id_Entidad, 
                existPoliticaAsignada.Id_Rol, 
                existPoliticaAsignada.Id_Proceso, 
                existPoliticaAsignada.FechaInicioAsignacion, 
                existPoliticaAsignada.FechaTerminoAsignacion, 
                existPoliticaAsignada.FechaCreacion, 
                existPoliticaAsignada.RolRequiereValidacion, 
                existPoliticaAsignada.RolAsignadoValidado, 
                existPoliticaAsignada.PoliticaAsignadaBase 
                );

         
            newPoliticaAsignada.CambiarRolAsignadoValidado(true);

            AddUpdateDomainEvent(command, newPoliticaAsignada, new PoliticaAsignadaEventAsignacionDeRolValidada(
                    newPoliticaAsignada.Id, 
                    newPoliticaAsignada.RolAsignadoValidado 
                    ), existPoliticaAsignada, newPoliticaAsignada);

            _politicaAsignadaRepository.Modificar(newPoliticaAsignada);
        
            CommandResponse.Data = new PoliticaAsignadaDTO(){
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
                };
            CommandResponse.Result = true;

            return await Commit(_politicaAsignadaRepository.UnitOfWork);
            }
    }
}

