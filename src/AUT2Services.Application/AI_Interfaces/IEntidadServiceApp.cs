// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.134
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Entidades;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IEntidadServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearEntidadViewModel data); 
    Task<CommandResponse> Modificar(ModificarEntidadViewModel data); 
    Task<CommandResponse> Eliminar(EliminarEntidadViewModel data); 
    Task<CommandResponse> FinalizaAutorizacion(FinalizaAutorizacionEntidadViewModel data); 
    Task<CommandResponse> CambiaEntidadAPrincipal(CambiaEntidadAPrincipalEntidadViewModel data); 
    Task<CommandResponse> CambiaEntidadANoPrincipal(CambiaEntidadANoPrincipalEntidadViewModel data); 
  
    Task<EntidadViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<EntidadViewModel> BuscarPor_Id_Usuario_Id_UnidadOrganizacional(
        Guid id_Usuario, 
        Guid id_UnidadOrganizacional 
        ); 

    Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_Usuario(
        Guid id_Usuario 
        ); 

    Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_UnidadOrganizacional(
        Guid id_UnidadOrganizacional 
        ); 

}

