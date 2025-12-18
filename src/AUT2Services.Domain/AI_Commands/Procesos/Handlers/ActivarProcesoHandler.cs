// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.123
// -------------------------------------------------
using AUT2Services.Domain.Commands.Procesos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.Procesos.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.Procesos.Handlers
{
    public partial class ProcesoCommandHandler :
        IRequestHandler<ActivarProcesoCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ActivarProcesoCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existProceso = await _procesoRepository.BuscarPor_Id(command.Id);

            if (existProceso is null)
            {
                AddError($"El Id de Proceso: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newProceso = new Proceso(
                existProceso.Id, 
                existProceso.IdMacro_Proceso, 
                existProceso.Codigo, 
                existProceso.Nombre, 
                existProceso.Descripcion, 
                existProceso.Contexto, 
                existProceso.NivelDeProceso, 
                existProceso.Url, 
                existProceso.Token, 
                existProceso.ComoDesplegarUrlDeProceso, 
                existProceso.ProcesoBase, 
                existProceso.MaximaAsignacionDeRoles, 
                existProceso.Activo 
                );

                if (existProceso.ProcesoBase)
        {
            AddError($"Los Procesos marcadas como Base no se pueden modificar");
            return CommandResponse;
        }

        if (existProceso.Activo)
        {
            AddError($"El Proceso ya se encuentra activado");
            return CommandResponse;
        }

        newProceso.CambiarActivo(true);

            newProceso.AddDomainEvent(new ProcesoEventActivada(
                    newProceso.Id, 
                    newProceso.Activo 
                    )
                );

            _procesoRepository.Modificar(newProceso);
        
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
}

