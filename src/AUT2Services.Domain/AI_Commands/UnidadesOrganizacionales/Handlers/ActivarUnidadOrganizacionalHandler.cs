// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.106
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Handlers
{
    public partial class UnidadOrganizacionalCommandHandler :
        IRequestHandler<ActivarUnidadOrganizacionalCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ActivarUnidadOrganizacionalCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existUnidadOrganizacional = await _unidadOrganizacionalRepository.BuscarPor_Id(command.Id);

            if (existUnidadOrganizacional is null)
            {
                AddError($"El Id de UnidadOrganizacional: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newUnidadOrganizacional = new UnidadOrganizacional(
                existUnidadOrganizacional.Id, 
                existUnidadOrganizacional.Id_Organizacion, 
                existUnidadOrganizacional.Codigo, 
                existUnidadOrganizacional.Nombre, 
                existUnidadOrganizacional.Descripcion, 
                existUnidadOrganizacional.UnidadOrganizacionalBase, 
                existUnidadOrganizacional.Activo 
                );

                    if (existUnidadOrganizacional.UnidadOrganizacionalBase)
            {
                AddError($"Las unidades organizacionales marcadas como Base no se pueden modificar");
                return CommandResponse;
            }

            if (existUnidadOrganizacional.Activo)
            {
                AddError($"La Unidad Organizacional ya se encuentra activa");
                return CommandResponse;
            }

            newUnidadOrganizacional.CambiarActivo(true);

            newUnidadOrganizacional.AddDomainEvent(new UnidadOrganizacionalEventActivado(
                    newUnidadOrganizacional.Id, 
                    newUnidadOrganizacional.Activo 
                    )
                );

            _unidadOrganizacionalRepository.Modificar(newUnidadOrganizacional);
        
            CommandResponse.Data = JsonConvert.SerializeObject(new UnidadOrganizacionalDTO(){
                    Id = newUnidadOrganizacional.Id, 
                    Id_Organizacion = newUnidadOrganizacional.Id_Organizacion, 
                    Codigo = newUnidadOrganizacional.Codigo, 
                    Nombre = newUnidadOrganizacional.Nombre, 
                    Descripcion = newUnidadOrganizacional.Descripcion, 
                    UnidadOrganizacionalBase = newUnidadOrganizacional.UnidadOrganizacionalBase, 
                    Activo = newUnidadOrganizacional.Activo 
                });
            CommandResponse.Result = true;

            return await Commit(_unidadOrganizacionalRepository.UnitOfWork);
            }
    }
}

