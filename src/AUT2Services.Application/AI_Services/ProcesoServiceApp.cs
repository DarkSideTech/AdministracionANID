// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.137
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Procesos;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class ProcesoServiceApp : IProcesoServiceApp
{
        private readonly IProcesoRepository _procesoRepository;
        private readonly IMediatorHandler _mediator;

    public ProcesoServiceApp(
                IProcesoRepository procesoRepository,
                IMediatorHandler mediator
        )
    {
                _procesoRepository = procesoRepository ?? throw new ArgumentNullException(nameof(procesoRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearProcesoViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarProcesoViewModel command)
    {
        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarProcesoViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> Activar(ActivarProcesoViewModel command)
    {
        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarProcesoViewModel command)
    {
        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<IEnumerable<ProcesoViewModel>> BuscarTodos( 
        )
    {
        return (await _procesoRepository.BuscarTodos( 
        )).ToViewModel(); 
    } 

    public async Task<ProcesoViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _procesoRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<ProcesoViewModel> BuscarPor_Codigo( 
        string codigo 
        )
    {
        return (await _procesoRepository.BuscarPor_Codigo( 
            codigo 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<ProcesoViewModel>> BuscarPor_IdMacro_Proceso( 
        Guid idMacro_Proceso 
        )
    {
        return (await _procesoRepository.BuscarPor_IdMacro_Proceso( 
            idMacro_Proceso 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

