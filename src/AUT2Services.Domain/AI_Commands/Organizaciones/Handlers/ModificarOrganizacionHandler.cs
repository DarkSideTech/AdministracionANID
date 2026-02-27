// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.273
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.Organizaciones.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.Organizaciones.Handlers
{
    public partial class OrganizacionCommandHandler :
        IRequestHandler<ModificarOrganizacionCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ModificarOrganizacionCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existOrganizacion = await _organizacionRepository.BuscarPor_Id(command.Id);

            if (existOrganizacion is null)
            {
                AddError($"El Id de Organizacion: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newOrganizacion = new Organizacion(
                existOrganizacion.Id, 
                command.IdOrganizacion, 
                existOrganizacion.Codigo, 
                command.Nombre, 
                command.Descripcion, 
                existOrganizacion.OrganizacionBase, 
                existOrganizacion.Activo 
                );

                    if (existOrganizacion.OrganizacionBase)
            {
                AddError($"Las Organizaciones marcadas como Base no se pueden modificar");
                return CommandResponse;
            }

            newOrganizacion.AddDomainEvent(new OrganizacionEventModificado(
                    newOrganizacion.Id, 
                    newOrganizacion.IdOrganizacion, 
                    newOrganizacion.Nombre, 
                    newOrganizacion.Descripcion 
                    )
                );

            _organizacionRepository.Modificar(newOrganizacion);
        
            CommandResponse.Data = JsonConvert.SerializeObject(new OrganizacionDTO(){
                    Id = newOrganizacion.Id, 
                    IdOrganizacion = newOrganizacion.IdOrganizacion, 
                    Codigo = newOrganizacion.Codigo, 
                    Nombre = newOrganizacion.Nombre, 
                    Descripcion = newOrganizacion.Descripcion, 
                    OrganizacionBase = newOrganizacion.OrganizacionBase, 
                    Activo = newOrganizacion.Activo 
                });
            CommandResponse.Result = true;

            return await Commit(_organizacionRepository.UnitOfWork);
            }
    }
}

