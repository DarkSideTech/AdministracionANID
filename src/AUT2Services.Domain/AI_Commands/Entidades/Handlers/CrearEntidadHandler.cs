// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.110
// -------------------------------------------------
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.Entidades.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.Entidades.Handlers;

public partial class EntidadCommandHandler :
    IRequestHandler<CrearEntidadCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearEntidadCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existEntidad = await _entidadRepository.BuscarPor_Id_Usuario_Id_UnidadOrganizacional(command.Id_Usuario, command.Id_UnidadOrganizacional);

        if(existEntidad is not null)
        {
            AddError($"Ya existe Entidad para la busqueda : Id_Usuario [{command.Id_Usuario}] Id_UnidadOrganizacional [{command.Id_UnidadOrganizacional}] ");
            return CommandResponse;
        }
        
        var newEntidad = new Entidad(
            Guid.NewGuid(), 
            command.Id_UnidadOrganizacional, 
            command.Id_Usuario, 
            command.TipoDeEntidad, 
            command.CorreoElectronico, 
            command.FechaInicioAutorizacion, 
            command.FechaTerminoAutorizacion, 
            command.FechaCreacion, 
            command.Principal, 
            command.EntidadBase 
        );

                newEntidad.CambiarFechaInicioAutorizacion(DateTimeOffset.Now);
        newEntidad.CambiarFechaTerminoAutorizacion(DateTimeOffset.MinValue);
        newEntidad.CambiarFechaCreacion(DateTimeOffset.Now);
        newEntidad.CambiarPrincipal(false);
        newEntidad.CambiarEntidadBase(false);

        newEntidad.AddDomainEvent(new EntidadEventCreado(
            newEntidad.Id, 
        newEntidad.Id_UnidadOrganizacional, 
        newEntidad.Id_Usuario, 
        newEntidad.TipoDeEntidad, 
        newEntidad.CorreoElectronico, 
        newEntidad.Principal, 
        newEntidad.EntidadBase 
            )
        );

        _entidadRepository.Crear(newEntidad);

        CommandResponse.Data = JsonConvert.SerializeObject(new EntidadDTO(){
            Id = newEntidad.Id, 
            Id_UnidadOrganizacional = newEntidad.Id_UnidadOrganizacional, 
            Id_Usuario = newEntidad.Id_Usuario, 
            TipoDeEntidad = newEntidad.TipoDeEntidad, 
            CorreoElectronico = newEntidad.CorreoElectronico, 
            FechaInicioAutorizacion = newEntidad.FechaInicioAutorizacion, 
            FechaTerminoAutorizacion = newEntidad.FechaTerminoAutorizacion, 
            FechaCreacion = newEntidad.FechaCreacion, 
            Principal = newEntidad.Principal, 
            EntidadBase = newEntidad.EntidadBase 
        });

        CommandResponse.Result = true;
        return await Commit(_entidadRepository.UnitOfWork);
        }
}

