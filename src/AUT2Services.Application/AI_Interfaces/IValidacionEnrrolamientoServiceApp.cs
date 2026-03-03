// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.915
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ValidacionEnrrolamientos;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IValidacionEnrrolamientoServiceApp : IDisposable
{
    Task<CommandResponse> Crear(CrearValidacionEnrrolamientoViewModel data); 
    Task<CommandResponse> Eliminar(EliminarValidacionEnrrolamientoViewModel data); 
    Task<CommandResponse> Activar(ActivarValidacionEnrrolamientoViewModel data); 
    Task<CommandResponse> Desactivar(DesactivarValidacionEnrrolamientoViewModel data); 
  
    Task<ValidacionEnrrolamientoViewModel> BuscarPor_Id(
        Guid id 
        ); 

    Task<ValidacionEnrrolamientoViewModel> BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario(
        Guid idValidado_Usuario, 
        Guid idValidaEnrrolamiento_Usuario 
        ); 

    Task<IEnumerable<ValidacionEnrrolamientoViewModel>> BuscarPor_IdValidado_Usuario(
        Guid idValidado_Usuario 
        ); 

    Task<IEnumerable<ValidacionEnrrolamientoViewModel>> BuscarPor_IdValidaEnrrolamiento_Usuario(
        Guid idValidaEnrrolamiento_Usuario 
        ); 

}

