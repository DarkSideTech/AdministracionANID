// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.425
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Proveedores;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class ProveedorServiceApp : IProveedorServiceApp
{
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IMediatorHandler _mediator;

    public ProveedorServiceApp(
                IProveedorRepository proveedorRepository,
                IMediatorHandler mediator
        )
    {
                _proveedorRepository = proveedorRepository ?? throw new ArgumentNullException(nameof(proveedorRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearProveedorViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarProveedorViewModel command)
    {
        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarProveedorViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> EliminarPor_Codigo(EliminarPor_CodigoProveedorViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarPor_CodigoCommand());
    }

    public async Task<CommandResponse> Activar(ActivarProveedorViewModel command)
    {
        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarProveedorViewModel command)
    {
        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<IEnumerable<ProveedorViewModel>> BuscarTodos( 
        )
    {
        return (await _proveedorRepository.BuscarTodos( 
        )).ToViewModel(); 
    } 

    public async Task<ProveedorViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _proveedorRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<ProveedorViewModel> BuscarPor_Codigo( 
        string codigo 
        )
    {
        return (await _proveedorRepository.BuscarPor_Codigo( 
            codigo 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

