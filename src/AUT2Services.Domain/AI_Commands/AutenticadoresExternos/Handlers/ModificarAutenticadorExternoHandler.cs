// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.253
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Events.AutenticadoresExternos.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Handlers
{
    public partial class AutenticadorExternoCommandHandler :
        IRequestHandler<ModificarAutenticadorExternoCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(ModificarAutenticadorExternoCommand command, CancellationToken cancellationToken)
        {
     
            CommandResponse.Result = false;
            if (!command.IsValid()) return command.CommandResponse;

            var existAutenticadorExterno = await _autenticadorExternoRepository.BuscarPor_Id(command.Id);

            if (existAutenticadorExterno is null)
            {
                AddError($"El Id de AutenticadorExterno: [{command.Id}], no Existe!");
                return CommandResponse;
            }
         
            var newAutenticadorExterno = new AutenticadorExterno(
                existAutenticadorExterno.Id, 
                existAutenticadorExterno.Id_Proveedor, 
                existAutenticadorExterno.Id_Usuario, 
                existAutenticadorExterno.NombreUsuario, 
                command.ClaveDeAcceso, 
                command.NombreADesplegar, 
                existAutenticadorExterno.ValidadorPrimario, 
                existAutenticadorExterno.AutenticadorExternoBase, 
                existAutenticadorExterno.Activo 
                );

                if (existAutenticadorExterno.AutenticadorExternoBase)
                {
                    AddError($"No es posible modificar un autenticador externo marcado como base");
                    return CommandResponse;
                }

            newAutenticadorExterno.AddDomainEvent(new AutenticadorExternoEventModificado(
                    newAutenticadorExterno.Id, 
                    newAutenticadorExterno.ClaveDeAcceso, 
                    newAutenticadorExterno.NombreADesplegar 
                    )
                );

            _autenticadorExternoRepository.Modificar(newAutenticadorExterno);
        
            CommandResponse.Data = JsonConvert.SerializeObject(new AutenticadorExternoDTO(){
                    Id = newAutenticadorExterno.Id, 
                    Id_Proveedor = newAutenticadorExterno.Id_Proveedor, 
                    Id_Usuario = newAutenticadorExterno.Id_Usuario, 
                    NombreUsuario = newAutenticadorExterno.NombreUsuario, 
                    ClaveDeAcceso = newAutenticadorExterno.ClaveDeAcceso, 
                    NombreADesplegar = newAutenticadorExterno.NombreADesplegar, 
                    ValidadorPrimario = newAutenticadorExterno.ValidadorPrimario, 
                    AutenticadorExternoBase = newAutenticadorExterno.AutenticadorExternoBase, 
                    Activo = newAutenticadorExterno.Activo 
                });
            CommandResponse.Result = true;

            return await Commit(_autenticadorExternoRepository.UnitOfWork);
            }
    }
}

