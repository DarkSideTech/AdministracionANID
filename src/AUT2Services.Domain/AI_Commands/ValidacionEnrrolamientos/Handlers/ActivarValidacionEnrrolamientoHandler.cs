// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.258
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;
using Newtonsoft.Json;

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

            newValidacionEnrrolamiento.AddDomainEvent(new ValidacionEnrrolamientoEventActivado(
                    newValidacionEnrrolamiento.Id, 
                    newValidacionEnrrolamiento.Activo 
                    )
                );

            _validacionEnrrolamientoRepository.Modificar(newValidacionEnrrolamiento);
        
            CommandResponse.Data = JsonConvert.SerializeObject(new ValidacionEnrrolamientoDTO(){
                    Id = newValidacionEnrrolamiento.Id, 
                    IdValidado_Usuario = newValidacionEnrrolamiento.IdValidado_Usuario, 
                    IdValidaEnrrolamiento_Usuario = newValidacionEnrrolamiento.IdValidaEnrrolamiento_Usuario, 
                    EnrrolamientoAceptado = newValidacionEnrrolamiento.EnrrolamientoAceptado, 
                    FechaValidacion = newValidacionEnrrolamiento.FechaValidacion, 
                    FechaRegistro = newValidacionEnrrolamiento.FechaRegistro, 
                    Activo = newValidacionEnrrolamiento.Activo 
                });
            CommandResponse.Result = true;

            return await Commit(_validacionEnrrolamientoRepository.UnitOfWork);
            }
    }
}

