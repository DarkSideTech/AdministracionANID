// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.129
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;
using System.Text.Json;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Handlers;

public partial class ValidacionEnrrolamientoCommandHandler :
    IRequestHandler<CrearValidacionEnrrolamientoCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearValidacionEnrrolamientoCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existValidacionEnrrolamiento = await _validacionEnrrolamientoRepository.BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario(command.IdValidado_Usuario, command.IdValidaEnrrolamiento_Usuario);

        if(existValidacionEnrrolamiento is not null)
        {
            AddError($"Ya existe ValidacionEnrrolamiento para la busqueda : IdValidado_Usuario [{command.IdValidado_Usuario}] IdValidaEnrrolamiento_Usuario [{command.IdValidaEnrrolamiento_Usuario}] ");
            return CommandResponse;
        }
        
        var newValidacionEnrrolamiento = new ValidacionEnrrolamiento(
            Guid.NewGuid(), 
            command.IdValidado_Usuario, 
            command.IdValidaEnrrolamiento_Usuario, 
            command.EnrrolamientoAceptado, 
            command.FechaValidacion, 
            command.FechaRegistro, 
            command.Activo 
        );

        newValidacionEnrrolamiento.CambiarFechaRegistro(_clock.UtcNow);
        newValidacionEnrrolamiento.CambiarActivo(true);

        AddCreateDomainEvent(command, newValidacionEnrrolamiento, new ValidacionEnrrolamientoEventCreado(
            newValidacionEnrrolamiento.Id, 
            newValidacionEnrrolamiento.IdValidado_Usuario, 
            newValidacionEnrrolamiento.IdValidaEnrrolamiento_Usuario, 
            newValidacionEnrrolamiento.EnrrolamientoAceptado, 
            newValidacionEnrrolamiento.FechaValidacion, 
            newValidacionEnrrolamiento.FechaRegistro 
            )
        , newValidacionEnrrolamiento);

        _validacionEnrrolamientoRepository.Crear(newValidacionEnrrolamiento);

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

