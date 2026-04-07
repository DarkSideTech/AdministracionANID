// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.429
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Organizaciones;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class OrganizacionServiceApp : IOrganizacionServiceApp
{
        private readonly IOrganizacionRepository _organizacionRepository;
        private readonly IMediatorHandler _mediator;

    public OrganizacionServiceApp(
                IOrganizacionRepository organizacionRepository,
                IMediatorHandler mediator
        )
    {
                _organizacionRepository = organizacionRepository ?? throw new ArgumentNullException(nameof(organizacionRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearOrganizacionViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> Modificar(ModificarOrganizacionViewModel command)
    {
        return await _mediator.SendCommand(command.ToModificarCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarOrganizacionViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> EliminarPor_Codigo(EliminarPor_CodigoOrganizacionViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarPor_CodigoCommand());
    }

    public async Task<CommandResponse> Activar(ActivarOrganizacionViewModel command)
    {
        return await _mediator.SendCommand(command.ToActivarCommand());
    }

    public async Task<CommandResponse> Desactivar(DesactivarOrganizacionViewModel command)
    {
        return await _mediator.SendCommand(command.ToDesactivarCommand());
    }

    public async Task<IEnumerable<OrganizacionViewModel>> BuscarTodos( 
        )
    {
        return (await _organizacionRepository.BuscarTodos( 
        )).ToViewModel(); 
    } 

    public async Task<OrganizacionViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _organizacionRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<OrganizacionViewModel> BuscarPor_Codigo( 
        string codigo 
        )
    {
        return (await _organizacionRepository.BuscarPor_Codigo( 
            codigo 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

