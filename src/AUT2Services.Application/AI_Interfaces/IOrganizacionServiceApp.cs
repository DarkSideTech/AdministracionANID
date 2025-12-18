// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.134
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Organizaciones;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IOrganizacionServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearOrganizacionViewModel data); 
    Task<CommandResponse> Modificar(ModificarOrganizacionViewModel data); 
    Task<CommandResponse> Eliminar(EliminarOrganizacionViewModel data); 
    Task<CommandResponse> EliminarPor_Codigo(EliminarPor_CodigoOrganizacionViewModel data); 
    Task<CommandResponse> Activar(ActivarOrganizacionViewModel data); 
    Task<CommandResponse> Desactivar(DesactivarOrganizacionViewModel data); 
  
    Task<IEnumerable<OrganizacionViewModel>> BuscarTodos(
        ); 

    Task<OrganizacionViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<OrganizacionViewModel> BuscarPor_Codigo(
        string codigo 
        ); 

}

