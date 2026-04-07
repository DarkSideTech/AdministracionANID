// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.423
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.UnidadesOrganizacionales;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IUnidadOrganizacionalServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearUnidadOrganizacionalViewModel data); 
    Task<CommandResponse> Modificar(ModificarUnidadOrganizacionalViewModel data); 
    Task<CommandResponse> Eliminar(EliminarUnidadOrganizacionalViewModel data); 
    Task<CommandResponse> EliminarPor_Codigo_Id_Organizacion(EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalViewModel data); 
    Task<CommandResponse> Activar(ActivarUnidadOrganizacionalViewModel data); 
    Task<CommandResponse> Desactivar(DesactivarUnidadOrganizacionalViewModel data); 
  
    Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarTodos(
        ); 

    Task<UnidadOrganizacionalViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<UnidadOrganizacionalViewModel> BuscarPor_Codigo_Id_Organizacion(
        string codigo, 
        Guid id_Organizacion 
        ); 

    Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarPor_Id_Organizacion(
        Guid id_Organizacion 
        ); 

}

