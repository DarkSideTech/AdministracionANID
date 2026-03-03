// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.917
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IServicioDeDominioServiceApp : IDisposable
{
    Task<CommandResponse> ValidaEnrrolamiento(ValidaEnrrolamientoServicioDeDominioViewModel data); 
    Task<CommandResponse> ValidaAsignacionDeRol(ValidaAsignacionDeRolServicioDeDominioViewModel data); 
    Task<CommandResponse> CrearEntidad(CrearEntidadServicioDeDominioViewModel data); 
    Task<CommandResponse> MarcarEntidadComoPrincipal(MarcarEntidadComoPrincipalServicioDeDominioViewModel data); 
  
    Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario, 
        Guid id_Organizacion 
        ); 

    Task<EntidadViewModel> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario, 
        Guid id_Organizacion 
        ); 

    Task<IEnumerable<OrganizacionPorUsuarioViewModel>> BuscarOrganizacionesPor_Usuario(
        ); 

}

