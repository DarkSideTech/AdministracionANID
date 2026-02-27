// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.300
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Proveedores;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IProveedorServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearProveedorViewModel data); 
    Task<CommandResponse> Modificar(ModificarProveedorViewModel data); 
    Task<CommandResponse> Eliminar(EliminarProveedorViewModel data); 
    Task<CommandResponse> EliminarPor_Codigo(EliminarPor_CodigoProveedorViewModel data); 
    Task<CommandResponse> Activar(ActivarProveedorViewModel data); 
    Task<CommandResponse> Desactivar(DesactivarProveedorViewModel data); 
  
    Task<IEnumerable<ProveedorViewModel>> BuscarTodos(
        ); 

    Task<ProveedorViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<ProveedorViewModel> BuscarPor_Codigo(
        string codigo 
        ); 

}

