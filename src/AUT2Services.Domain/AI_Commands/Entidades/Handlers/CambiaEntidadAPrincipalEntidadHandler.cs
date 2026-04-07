// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.140
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
        IRequestHandler<CambiaEntidadAPrincipalEntidadCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(CambiaEntidadAPrincipalEntidadCommand command, CancellationToken cancellationToken)
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
                existEntidad.CorreoElectronico, 
                existEntidad.FechaInicioAutorizacion, 
                existEntidad.FechaTerminoAutorizacion, 
                existEntidad.FechaCreacion, 
                existEntidad.Principal, 
                existEntidad.EntidadBase 
                );

            var existEntidadPrincipal = await _entidadRepository.BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal(existEntidad.Id_Usuario, existEntidad.Id_UnidadOrganizacional);

            if (existEntidadPrincipal is not null)
            {
                var newEntidadPrincipal = new Entidad(
                    existEntidadPrincipal.Id,
                    existEntidadPrincipal.Id_UnidadOrganizacional,
                    existEntidadPrincipal.Id_Usuario,
                    existEntidadPrincipal.TipoDeEntidad,
                    existEntidadPrincipal.CorreoElectronico,
                    existEntidadPrincipal.FechaInicioAutorizacion,
                    existEntidadPrincipal.FechaTerminoAutorizacion,
                    existEntidadPrincipal.FechaCreacion,
                    false,
                    existEntidadPrincipal.EntidadBase
                    );

                newEntidadPrincipal.AddDomainEvent(new EntidadEventEntidadCambiadaAPrincipal(
                    newEntidadPrincipal.Id,
                    newEntidadPrincipal.Principal
                    )
                );

                _entidadRepository.Modificar(newEntidadPrincipal);
            }

            newEntidad.CambiarPrincipal(true);

            newEntidad.AddDomainEvent(new EntidadEventEntidadCambiadaAPrincipal(
                    newEntidad.Id, 
                    newEntidad.Principal 
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

