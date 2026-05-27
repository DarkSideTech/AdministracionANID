// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.427
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.UnidadesOrganizacionales;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class UnidadOrganizacionalServiceApp : IUnidadOrganizacionalServiceApp
{
        private readonly IUnidadOrganizacionalRepository _unidadOrganizacionalRepository;
        private readonly IMediatorHandler _mediator;

    public UnidadOrganizacionalServiceApp(
                IUnidadOrganizacionalRepository unidadOrganizacionalRepository,
                IMediatorHandler mediator
        )
    {
                _unidadOrganizacionalRepository = unidadOrganizacionalRepository ?? throw new ArgumentNullException(nameof(unidadOrganizacionalRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearUnidadOrganizacionalViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarUnidadOrganizacionalViewModel command)
    {
        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarUnidadOrganizacionalViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> EliminarPor_Codigo_Id_Organizacion(EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarPor_Codigo_Id_OrganizacionCommand());
    }

    public async Task<CommandResponse> Activar(ActivarUnidadOrganizacionalViewModel command)
    {
        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarUnidadOrganizacionalViewModel command)
    {
        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarTodos( 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarTodos( 
        )).ToViewModel(); 
    } 

    public async Task<UnidadOrganizacionalViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<UnidadOrganizacionalViewModel> BuscarPor_Codigo_Id_Organizacion( 
        string codigo, 
        Guid id_Organizacion 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarPor_Codigo_Id_Organizacion( 
            codigo, 
            id_Organizacion 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarPor_Id_Organizacion( 
        Guid id_Organizacion 
        )
    {
        return (await _unidadOrganizacionalRepository.BuscarPor_Id_Organizacion( 
            id_Organizacion 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

