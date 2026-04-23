// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.130
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;
using System.Text.Json;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Handlers
{
    public partial class ValidacionEnrrolamientoCommandHandler :
        IRequestHandler<ActivarValidacionEnrrolamientoCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ActivarValidacionEnrrolamientoCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existValidacionEnrrolamiento = await _validacionEnrrolamientoRepository.BuscarPor_Id(command.Id);

            if (existValidacionEnrrolamiento is null)
            {
                AddError($"El Id de ValidacionEnrrolamiento: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newValidacionEnrrolamiento = new ValidacionEnrrolamiento(
                existValidacionEnrrolamiento.Id, 
                existValidacionEnrrolamiento.IdValidado_Usuario, 
                existValidacionEnrrolamiento.IdValidaEnrrolamiento_Usuario, 
                existValidacionEnrrolamiento.EnrrolamientoAceptado, 
                existValidacionEnrrolamiento.FechaValidacion, 
                existValidacionEnrrolamiento.FechaRegistro, 
                existValidacionEnrrolamiento.Activo 
                );

         
            newValidacionEnrrolamiento.CambiarActivo(true);

            AddUpdateDomainEvent(command, newValidacionEnrrolamiento, new ValidacionEnrrolamientoEventActivado(
                    newValidacionEnrrolamiento.Id, 
                    newValidacionEnrrolamiento.Activo 
                    ), existValidacionEnrrolamiento, newValidacionEnrrolamiento);

            _validacionEnrrolamientoRepository.Modificar(newValidacionEnrrolamiento);
        
            CommandResponse.Data = new ValidacionEnrrolamientoDTO(){
                    Id = newValidacionEnrrolamiento.Id, 
                    IdValidado_Usuario = newValidacionEnrrolamiento.IdValidado_Usuario, 
                    IdValidaEnrrolamiento_Usuario = newValidacionEnrrolamiento.IdValidaEnrrolamiento_Usuario, 
                    EnrrolamientoAceptado = newValidacionEnrrolamiento.EnrrolamientoAceptado, 
                    FechaValidacion = newValidacionEnrrolamiento.FechaValidacion, 
                    FechaRegistro = newValidacionEnrrolamiento.FechaRegistro, 
                    Activo = newValidacionEnrrolamiento.Activo 
                };
            CommandResponse.Result = true;

            return await Commit(_validacionEnrrolamientoRepository.UnitOfWork);
            }
    }
}

