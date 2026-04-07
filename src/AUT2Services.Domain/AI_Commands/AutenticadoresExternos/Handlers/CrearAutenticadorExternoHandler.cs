// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.125
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Events.AutenticadoresExternos.Events;
using Newtonsoft.Json;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Handlers;

public partial class AutenticadorExternoCommandHandler :
    IRequestHandler<CrearAutenticadorExternoCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(CrearAutenticadorExternoCommand command, CancellationToken cancellationToken)
    {
     
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        if (!command.IsValid())
        {
            return CommandResponse;
        }

        var existAutenticadorExterno = await _autenticadorExternoRepository.BuscarPor_Id_Proveedor_Id_Usuario(command.Id_Proveedor, command.Id_Usuario);

        if(existAutenticadorExterno is not null)
        {
            AddError($"Ya existe AutenticadorExterno para la busqueda : Id_Proveedor [{command.Id_Proveedor}] Id_Usuario [{command.Id_Usuario}] ");
            return CommandResponse;
        }
        
        var newAutenticadorExterno = new AutenticadorExterno(
            Guid.NewGuid(), 
            command.Id_Proveedor, 
            command.Id_Usuario, 
            command.NombreUsuario, 
            command.ClaveDeAcceso, 
            command.NombreADesplegar, 
            command.ValidadorPrimario, 
            command.AutenticadorExternoBase, 
            command.Activo 
        );

                newAutenticadorExterno.CambiarActivo(true);

        var existAutenticadorExternoAnterior = await _autenticadorExternoRepository.BuscarPor_Id_Usuario_ValidadorPrimario(command.Id_Usuario);

        if (existAutenticadorExternoAnterior is null)
        {
            newAutenticadorExterno.CambiarValidadorPrimario(true);
        }

        newAutenticadorExterno.AddDomainEvent(new AutenticadorExternoEventCreado(
            newAutenticadorExterno.Id, 
        newAutenticadorExterno.Id_Proveedor, 
        newAutenticadorExterno.Id_Usuario, 
        newAutenticadorExterno.NombreUsuario, 
        newAutenticadorExterno.ClaveDeAcceso, 
        newAutenticadorExterno.NombreADesplegar 
            )
        );

        _autenticadorExternoRepository.Crear(newAutenticadorExterno);

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

