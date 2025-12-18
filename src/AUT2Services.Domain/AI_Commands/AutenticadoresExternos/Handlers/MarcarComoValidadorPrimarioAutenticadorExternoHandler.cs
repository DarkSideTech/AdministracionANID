// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.099
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
        IRequestHandler<MarcarComoValidadorPrimarioAutenticadorExternoCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(MarcarComoValidadorPrimarioAutenticadorExternoCommand command, CancellationToken cancellationToken)
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
                existAutenticadorExterno.ClaveDeAcceso, 
                existAutenticadorExterno.NombreADesplegar, 
                existAutenticadorExterno.ValidadorPrimario, 
                existAutenticadorExterno.AutenticadorExternoBase, 
                existAutenticadorExterno.Activo 
                );

                    var existAutenticadorExternoAnterior = await _autenticadorExternoRepository.BuscarPor_Id_Usuario_ValidadorPrimario(existAutenticadorExterno.Id_Usuario);

            if (existAutenticadorExternoAnterior is not null)
            {
                if (existAutenticadorExterno.Id.Equals(existAutenticadorExternoAnterior.Id))
                {
                    AddError($"El autenticador externo ya es validador primario");
                    return CommandResponse;
                }

                var newAutenticadorExternoAnterior = new AutenticadorExterno(
                    existAutenticadorExternoAnterior.Id,
                    existAutenticadorExternoAnterior.Id_Proveedor,
                    existAutenticadorExternoAnterior.Id_Usuario,
                    existAutenticadorExternoAnterior.NombreUsuario,
                    existAutenticadorExternoAnterior.ClaveDeAcceso,
                    existAutenticadorExternoAnterior.NombreADesplegar,
                    false,
                    existAutenticadorExterno.AutenticadorExternoBase, 
                    existAutenticadorExternoAnterior.Activo
                    );

                newAutenticadorExternoAnterior.AddDomainEvent(new AutenticadorExternoEventValidadorPrimarioMarcado(
                    newAutenticadorExternoAnterior.Id,
                    newAutenticadorExternoAnterior.ValidadorPrimario
                    )
                );

                _autenticadorExternoRepository.Modificar(newAutenticadorExternoAnterior);
            }
            
            newAutenticadorExterno.CambiarValidadorPrimario(true);


            newAutenticadorExterno.AddDomainEvent(new AutenticadorExternoEventValidadorPrimarioMarcado(
                    newAutenticadorExterno.Id, 
                    newAutenticadorExterno.ValidadorPrimario 
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

