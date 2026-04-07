// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.429
// -------------------------------------------------
using AUT2Services.Application.Extensions;
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.PoliticasAsignadas;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Application.Services;

public class PoliticaAsignadaServiceApp : IPoliticaAsignadaServiceApp
{
        private readonly IPoliticaAsignadaRepository _politicaAsignadaRepository;
        private readonly IMediatorHandler _mediator;

    public PoliticaAsignadaServiceApp(
                IPoliticaAsignadaRepository politicaAsignadaRepository,
                IMediatorHandler mediator
        )
    {
                _politicaAsignadaRepository = politicaAsignadaRepository ?? throw new ArgumentNullException(nameof(politicaAsignadaRepository));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<CommandResponse> Crear(CrearPoliticaAsignadaViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearCommand());
    }

    public async Task<CommandResponse> CrearAsignarNuevaEntidadPersona(CrearAsignarNuevaEntidadPersonaPoliticaAsignadaViewModel command)
    {
        return await _mediator.SendCommand(command.ToCrearAsignarNuevaEntidadPersonaCommand());
    }

    public async Task<CommandResponse> Eliminar(EliminarPoliticaAsignadaViewModel command)
    {
        return await _mediator.SendCommand(command.ToEliminarCommand());
    }

    public async Task<CommandResponse> FinalizaAsignacion(FinalizaAsignacionPoliticaAsignadaViewModel command)
    {
        return await _mediator.SendCommand(command.ToFinalizaAsignacionCommand());
    }

    public async Task<CommandResponse> ValidaAsignacionDeRol(ValidaAsignacionDeRolPoliticaAsignadaViewModel command)
    {
        return await _mediator.SendCommand(command.ToValidaAsignacionDeRolCommand());
    }

    public async Task<PoliticaAsignadaViewModel> BuscarPor_Id( 
        Guid id 
        )
    {
        return (await _politicaAsignadaRepository.BuscarPor_Id( 
            id 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Entidad_Id_Rol_Id_Proceso( 
        Guid id_Entidad, 
        Guid id_Rol, 
        Guid id_Proceso 
        )
    {
        return (await _politicaAsignadaRepository.BuscarPor_Id_Entidad_Id_Rol_Id_Proceso( 
            id_Entidad, 
            id_Rol, 
            id_Proceso 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Entidad( 
        Guid id_Entidad 
        )
    {
        return (await _politicaAsignadaRepository.BuscarPor_Id_Entidad( 
            id_Entidad 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Rol( 
        Guid id_Rol 
        )
    {
        return (await _politicaAsignadaRepository.BuscarPor_Id_Rol( 
            id_Rol 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Proceso( 
        Guid id_Proceso 
        )
    {
        return (await _politicaAsignadaRepository.BuscarPor_Id_Proceso( 
            id_Proceso 
        )).ToViewModel(); 
    } 

    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_RolRequiereValidacion( 
        )
    {
        return (await _politicaAsignadaRepository.BuscarPor_RolRequiereValidacion( 
        )).ToViewModel(); 
    } 

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

