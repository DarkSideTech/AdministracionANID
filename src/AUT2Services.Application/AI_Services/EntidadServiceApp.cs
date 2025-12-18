// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.136
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Entidades;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class EntidadServiceApp : IEntidadServiceApp
{
        private readonly IEntidadRepository _entidadRepository;
        private readonly IMediatorHandler _mediator;

    public EntidadServiceApp(
                IEntidadRepository entidadRepository,
                IMediatorHandler mediator
        )
    {
                _entidadRepository = entidadRepository ?? throw new ArgumentNullException(nameof(entidadRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearEntidadViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarEntidadViewModel command)
    {
        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarEntidadViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> FinalizaAutorizacion(FinalizaAutorizacionEntidadViewModel command)
    {
        return await _mediator.SendCommand(command.ToFinalizaAutorizacionCommand());
    }

    public async Task<CommandResponse> CambiaEntidadAPrincipal(CambiaEntidadAPrincipalEntidadViewModel command)
    {
        return await _mediator.SendCommand(command.ToCambiaEntidadAPrincipalCommand());
    }

    public async Task<CommandResponse> CambiaEntidadANoPrincipal(CambiaEntidadANoPrincipalEntidadViewModel command)
    {
        return await _mediator.SendCommand(command.ToCambiaEntidadANoPrincipalCommand());
    }

    public async Task<EntidadViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _entidadRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<EntidadViewModel> BuscarPor_Id_Usuario_Id_UnidadOrganizacional( 
        Guid id_Usuario, 
        Guid id_UnidadOrganizacional 
        )
    {
        return (await _entidadRepository.BuscarPor_Id_Usuario_Id_UnidadOrganizacional( 
            id_Usuario, 
            id_UnidadOrganizacional 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_Usuario( 
        Guid id_Usuario 
        )
    {
        return (await _entidadRepository.BuscarPor_Id_Usuario( 
            id_Usuario 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_UnidadOrganizacional( 
        Guid id_UnidadOrganizacional 
        )
    {
        return (await _entidadRepository.BuscarPor_Id_UnidadOrganizacional( 
            id_UnidadOrganizacional 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

