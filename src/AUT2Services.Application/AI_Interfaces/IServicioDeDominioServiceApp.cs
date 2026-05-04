// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.424
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Application.Interfaces;

public interface IServicioDeDominioServiceApp : IDisposable
{
    Task<CommandResponse> BuscarUsuariosPendientesEnrrolamiento(BuscarUsuariosPendientesEnrrolamientoServicioDeDominioViewModel data);
    Task<CommandResponse> BuscarAsignacionesRolesPendientesValidacion(BuscarAsignacionesRolesPendientesValidacionServicioDeDominioViewModel data);
    Task<CommandResponse> ValidaEnrrolamiento(ValidaEnrrolamientoServicioDeDominioViewModel data); 
    Task<CommandResponse> ValidaAsignacionDeRol(ValidaAsignacionDeRolServicioDeDominioViewModel data); 
    Task<CommandResponse> CrearEntidad(CrearEntidadServicioDeDominioViewModel data); 
    Task<CommandResponse> EliminarEntidad(EliminarEntidadServicioDeDominioViewModel data); 
    Task<CommandResponse> SincronizarPoliticasAsignadas(SincronizarPoliticasAsignadasServicioDeDominioViewModel data);
    Task<CommandResponse> BuscarUnidadesOrganizacionalesParaAsignarOrganizacion(BuscarUnidadesOrganizacionalesParaAsignarOrganizacionServicioDeDominioViewModel data);
    Task<CommandResponse> SincronizarUnidadesOrganizacionalesOrganizacion(SincronizarUnidadesOrganizacionalesOrganizacionServicioDeDominioViewModel data);
    Task<CommandResponse> MarcarEntidadComoPrincipal(MarcarEntidadComoPrincipalServicioDeDominioViewModel data); 
  
    Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario, 
        Guid id_Organizacion 
        ); 

    Task<IEnumerable<EntidadParaAsignarPoliticaViewModel>> BuscarEntidadesParaAsignarPolitica(
        Guid id_Usuario,
        Guid id_UnidadOrganizacional
        );

    Task<IEnumerable<EntidadParaAsignarPoliticaViewModel>> BuscarEntidadesParaAsignarPoliticaPorOrganizacion(
        Guid id_Organizacion
        );

    Task<IEnumerable<PoliticaAsignadaParaEntidadViewModel>> BuscarPoliticasAsignadasPorEntidad(
        Guid id_Entidad
        );

    Task<EntidadViewModel?> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario, 
        Guid id_Organizacion 
        ); 

    Task<IEnumerable<OrganizacionPorUsuarioViewModel>> BuscarOrganizacionesPor_Usuario(
        ); 

}

