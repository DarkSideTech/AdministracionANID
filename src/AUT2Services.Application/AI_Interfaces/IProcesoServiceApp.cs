// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.135
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Procesos;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IProcesoServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearProcesoViewModel data); 
    Task<CommandResponse> Modificar(ModificarProcesoViewModel data); 
    Task<CommandResponse> Eliminar(EliminarProcesoViewModel data); 
    Task<CommandResponse> Activar(ActivarProcesoViewModel data); 
    Task<CommandResponse> Desactivar(DesactivarProcesoViewModel data); 
  
    Task<IEnumerable<ProcesoViewModel>> BuscarTodos(
        ); 

    Task<ProcesoViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<ProcesoViewModel> BuscarPor_Codigo(
        string codigo 
        ); 

    Task<IEnumerable<ProcesoViewModel>> BuscarPor_IdMacro_Proceso(
        Guid idMacro_Proceso 
        ); 

}

