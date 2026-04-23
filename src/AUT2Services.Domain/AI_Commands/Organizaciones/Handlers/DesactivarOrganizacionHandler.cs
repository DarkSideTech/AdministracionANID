// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.146
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.Organizaciones.Events;
using System.Text.Json;

namespace AUT2Services.Domain.Commands.Organizaciones.Handlers
{
    public partial class OrganizacionCommandHandler :
        IRequestHandler<DesactivarOrganizacionCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(DesactivarOrganizacionCommand command, CancellationToken cancellationToken)
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
                existOrganizacion.IdOrganizacion, 
                existOrganizacion.Codigo, 
                existOrganizacion.Nombre, 
                existOrganizacion.Descripcion, 
                existOrganizacion.OrganizacionBase, 
                existOrganizacion.Activo 
                );

                    if (existOrganizacion.OrganizacionBase)
            {
                AddError($"Las Organizaciones marcadas como Base no se pueden modificar");
                return CommandResponse;
            }

            if (!existOrganizacion.Activo)
            {
                AddError($"La Organizacion ya se encuentra desactivada");
                return CommandResponse;
            }

            newOrganizacion.CambiarActivo(false);

            AddUpdateDomainEvent(command, newOrganizacion, new OrganizacionEventDesactivado(
                    newOrganizacion.Id, 
                    newOrganizacion.Activo 
                    ), existOrganizacion, newOrganizacion);

            _organizacionRepository.Modificar(newOrganizacion);
        
            CommandResponse.Data = new OrganizacionDTO(){
                    Id = newOrganizacion.Id, 
                    IdOrganizacion = newOrganizacion.IdOrganizacion, 
                    Codigo = newOrganizacion.Codigo, 
                    Nombre = newOrganizacion.Nombre, 
                    Descripcion = newOrganizacion.Descripcion, 
                    OrganizacionBase = newOrganizacion.OrganizacionBase, 
                    Activo = newOrganizacion.Activo 
                };
            CommandResponse.Result = true;

            return await Commit(_organizacionRepository.UnitOfWork);
            }
    }
}

