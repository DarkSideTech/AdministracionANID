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

namespace AUT2Services.Domain.Commands.Entidades.Handlers
{
    public partial class EntidadCommandHandler :
        IRequestHandler<ModificarEntidadCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ModificarEntidadCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existEntidad = await _entidadRepository.BuscarPor_Id(command.Id);

            if (existEntidad is null)
            {
                AddError($"El Id de Entidad: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newEntidad = new Entidad(
                existEntidad.Id, 
                existEntidad.Id_UnidadOrganizacional, 
                existEntidad.Id_Usuario, 
                existEntidad.TipoDeEntidad, 
                command.CorreoElectronico, 
                existEntidad.FechaInicioAutorizacion, 
                existEntidad.FechaTerminoAutorizacion, 
                existEntidad.FechaCreacion, 
                existEntidad.Principal, 
                existEntidad.EntidadBase 
                );

                    if (existEntidad.EntidadBase)
            {
                AddError($"Los entidades marcados como Base no se pueden modificar");
                return CommandResponse;
            }

            newEntidad.AddDomainEvent(new EntidadEventModificado(
                    newEntidad.Id, 
                    newEntidad.CorreoElectronico 
                    )
                );

            _entidadRepository.Modificar(newEntidad);
        
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
}

