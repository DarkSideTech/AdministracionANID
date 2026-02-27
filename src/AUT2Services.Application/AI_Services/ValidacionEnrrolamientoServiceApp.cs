// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.305
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ValidacionEnrrolamientos;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class ValidacionEnrrolamientoServiceApp : IValidacionEnrrolamientoServiceApp
{
        private readonly IValidacionEnrrolamientoRepository _validacionEnrrolamientoRepository;
        private readonly IMediatorHandler _mediator;

    public ValidacionEnrrolamientoServiceApp(
                IValidacionEnrrolamientoRepository validacionEnrrolamientoRepository,
                IMediatorHandler mediator
        )
    {
                _validacionEnrrolamientoRepository = validacionEnrrolamientoRepository ?? throw new ArgumentNullException(nameof(validacionEnrrolamientoRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearValidacionEnrrolamientoViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarValidacionEnrrolamientoViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> Activar(ActivarValidacionEnrrolamientoViewModel command)
    {
        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarValidacionEnrrolamientoViewModel command)
    {
        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<ValidacionEnrrolamientoViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _validacionEnrrolamientoRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<ValidacionEnrrolamientoViewModel> BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario( 
        Guid idValidado_Usuario, 
        Guid idValidaEnrrolamiento_Usuario 
        )
    {
        return (await _validacionEnrrolamientoRepository.BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario( 
            idValidado_Usuario, 
            idValidaEnrrolamiento_Usuario 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<ValidacionEnrrolamientoViewModel>> BuscarPor_IdValidado_Usuario( 
        Guid idValidado_Usuario 
        )
    {
        return (await _validacionEnrrolamientoRepository.BuscarPor_IdValidado_Usuario( 
            idValidado_Usuario 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<ValidacionEnrrolamientoViewModel>> BuscarPor_IdValidaEnrrolamiento_Usuario( 
        Guid idValidaEnrrolamiento_Usuario 
        )
    {
        return (await _validacionEnrrolamientoRepository.BuscarPor_IdValidaEnrrolamiento_Usuario( 
            idValidaEnrrolamiento_Usuario 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

