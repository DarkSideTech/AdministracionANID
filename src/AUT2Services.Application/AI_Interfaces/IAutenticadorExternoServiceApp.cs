// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.422
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.AutenticadoresExternos;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IAutenticadorExternoServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearAutenticadorExternoViewModel data); 
    Task<CommandResponse> Modificar(ModificarAutenticadorExternoViewModel data); 
    Task<CommandResponse> Eliminar(EliminarAutenticadorExternoViewModel data); 
    Task<CommandResponse> MarcarComoValidadorPrimario(MarcarComoValidadorPrimarioAutenticadorExternoViewModel data); 
    Task<CommandResponse> Activar(ActivarAutenticadorExternoViewModel data); 
    Task<CommandResponse> Desactivar(DesactivarAutenticadorExternoViewModel data); 
  
    Task<AutenticadorExternoViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<IEnumerable<AutenticadorExternoViewModel>> BuscarPor_Id_Proveedor(
        Guid id_Proveedor 
        ); 

    Task<IEnumerable<AutenticadorExternoViewModel>> BuscarPor_Id_Usuario(
        Guid id_Usuario 
        ); 

    Task<AutenticadorExternoViewModel> BuscarPor_Id_Usuario_ValidadorPrimario(
        Guid id_Usuario 
        ); 

    Task<AutenticadorExternoViewModel> BuscarPor_Id_Proveedor_Id_Usuario(
        Guid id_Proveedor, 
        Guid id_Usuario 
        ); 

}

