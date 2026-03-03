// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.919
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.AutenticadoresExternos;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class AutenticadorExternoServiceApp : IAutenticadorExternoServiceApp
{
        private readonly IAutenticadorExternoRepository _autenticadorExternoRepository;
        private readonly IMediatorHandler _mediator;

    public AutenticadorExternoServiceApp(
                IAutenticadorExternoRepository autenticadorExternoRepository,
                IMediatorHandler mediator
        )
    {
                _autenticadorExternoRepository = autenticadorExternoRepository ?? throw new ArgumentNullException(nameof(autenticadorExternoRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearAutenticadorExternoViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarAutenticadorExternoViewModel command)
    {
        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarAutenticadorExternoViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> MarcarComoValidadorPrimario(MarcarComoValidadorPrimarioAutenticadorExternoViewModel command)
    {
        return await _mediator.SendCommand(command.ToMarcarComoValidadorPrimarioCommand());
    }

    public async Task<CommandResponse> Activar(ActivarAutenticadorExternoViewModel command)
    {
        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarAutenticadorExternoViewModel command)
    {
        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<AutenticadorExternoViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _autenticadorExternoRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<AutenticadorExternoViewModel>> BuscarPor_Id_Proveedor( 
        Guid id_Proveedor 
        )
    {
        return (await _autenticadorExternoRepository.BuscarPor_Id_Proveedor( 
            id_Proveedor 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<AutenticadorExternoViewModel>> BuscarPor_Id_Usuario( 
        Guid id_Usuario 
        )
    {
        return (await _autenticadorExternoRepository.BuscarPor_Id_Usuario( 
            id_Usuario 
        )).ToViewModel(); 
    } 

    public async Task<AutenticadorExternoViewModel> BuscarPor_Id_Usuario_ValidadorPrimario( 
        Guid id_Usuario 
        )
    {
        return (await _autenticadorExternoRepository.BuscarPor_Id_Usuario_ValidadorPrimario( 
            id_Usuario 
        )).ToViewModel(); 
    } 

    public async Task<AutenticadorExternoViewModel> BuscarPor_Id_Proveedor_Id_Usuario( 
        Guid id_Proveedor, 
        Guid id_Usuario 
        )
    {
        return (await _autenticadorExternoRepository.BuscarPor_Id_Proveedor_Id_Usuario( 
            id_Proveedor, 
            id_Usuario 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

