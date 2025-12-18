// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.135
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.PoliticasAsignadas;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IPoliticaAsignadaServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearPoliticaAsignadaViewModel data); 
    Task<CommandResponse> CrearAsignarNuevaEntidadPersona(CrearAsignarNuevaEntidadPersonaPoliticaAsignadaViewModel data); 
    Task<CommandResponse> Eliminar(EliminarPoliticaAsignadaViewModel data); 
    Task<CommandResponse> FinalizaAsignacion(FinalizaAsignacionPoliticaAsignadaViewModel data); 
    Task<CommandResponse> ValidaAsignacionDeRol(ValidaAsignacionDeRolPoliticaAsignadaViewModel data); 
  
    Task<PoliticaAsignadaViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Entidad_Id_Rol_Id_Proceso(
        Guid id_Entidad, 
        Guid id_Rol, 
        Guid id_Proceso 
        ); 

    Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Entidad(
        Guid id_Entidad 
        ); 

    Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Rol(
        Guid id_Rol 
        ); 

    Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Proceso(
        Guid id_Proceso 
        ); 

    Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_RolRequiereValidacion(
        ); 

}

