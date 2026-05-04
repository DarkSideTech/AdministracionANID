// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.456
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class ServicioDeDominioController : ApiController
{
    private readonly IServicioDeDominioServiceApp _servicioDeDominioServiceApp;
    private readonly ILogger<ServicioDeDominioController> _logger;

    public ServicioDeDominioController(
        IServicioDeDominioServiceApp servicioDeDominioServiceApp, 
        ILogger<ServicioDeDominioController> logger)
    {
        _servicioDeDominioServiceApp = servicioDeDominioServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ENRROLAMIENTO)]
    [HttpPost("BuscarUsuariosPendientesEnrrolamiento")]
    public async Task<IActionResult> BuscarUsuariosPendientesEnrrolamiento(BuscarUsuariosPendientesEnrrolamientoServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.BuscarUsuariosPendientesEnrrolamiento(dataViewModel));
    }

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ASIGNACION_ROLES)]
    [HttpPost("BuscarAsignacionesRolesPendientesValidacion")]
    public async Task<IActionResult> BuscarAsignacionesRolesPendientesValidacion(BuscarAsignacionesRolesPendientesValidacionServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.BuscarAsignacionesRolesPendientesValidacion(dataViewModel));
    }

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ENRROLAMIENTO)]
    [HttpPost("ValidaEnrrolamiento")]
    public async Task<IActionResult> ValidaEnrrolamiento(ValidaEnrrolamientoServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.ValidaEnrrolamiento(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ASIGNACION_ROLES)]
    [HttpPost("ValidaAsignacionDeRol")]
    public async Task<IActionResult> ValidaAsignacionDeRol(ValidaAsignacionDeRolServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.ValidaAsignacionDeRol(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPost("CrearEntidad")]
    public async Task<IActionResult> CrearEntidad(CrearEntidadServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.CrearEntidad(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPost("EliminarEntidad")]
    public async Task<IActionResult> EliminarEntidad(EliminarEntidadServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.EliminarEntidad(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPost("SincronizarPoliticasAsignadas")]
    public async Task<IActionResult> SincronizarPoliticasAsignadas(SincronizarPoliticasAsignadasServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.SincronizarPoliticasAsignadas(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPost("BuscarUnidadesOrganizacionalesParaAsignarOrganizacion")]
    public async Task<IActionResult> BuscarUnidadesOrganizacionalesParaAsignarOrganizacion(BuscarUnidadesOrganizacionalesParaAsignarOrganizacionServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.BuscarUnidadesOrganizacionalesParaAsignarOrganizacion(dataViewModel));
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPost("SincronizarUnidadesOrganizacionalesOrganizacion")]
    public async Task<IActionResult> SincronizarUnidadesOrganizacionalesOrganizacion(SincronizarUnidadesOrganizacionalesOrganizacionServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.SincronizarUnidadesOrganizacionalesOrganizacion(dataViewModel));
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD_USUARIO)]
    [HttpPost("MarcarEntidadComoPrincipal")]
    public async Task<IActionResult> MarcarEntidadComoPrincipal(MarcarEntidadComoPrincipalServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.MarcarEntidadComoPrincipal(dataViewModel));
    } 

    [Authorize]
    [HttpGet("BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion")]
    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion( 
            Guid id_Usuario, 
            Guid id_Organizacion 
        ) 
    {
        return await _servicioDeDominioServiceApp.BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion( 
            id_Usuario, 
            id_Organizacion 
        ); 
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpGet("BuscarEntidadesParaAsignarPolitica")]
    public async Task<IEnumerable<EntidadParaAsignarPoliticaViewModel>> BuscarEntidadesParaAsignarPolitica(
            Guid id_Usuario,
            Guid id_UnidadOrganizacional
        )
    {
        return await _servicioDeDominioServiceApp.BuscarEntidadesParaAsignarPolitica(
            id_Usuario,
            id_UnidadOrganizacional
        );
    }

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpGet("BuscarEntidadesParaAsignarPoliticaPorOrganizacion")]
    public async Task<IEnumerable<EntidadParaAsignarPoliticaViewModel>> BuscarEntidadesParaAsignarPoliticaPorOrganizacion(
            Guid id_Organizacion
        )
    {
        return await _servicioDeDominioServiceApp.BuscarEntidadesParaAsignarPoliticaPorOrganizacion(
            id_Organizacion
        );
    }

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpGet("BuscarPoliticasAsignadasPorEntidad")]
    public async Task<IEnumerable<PoliticaAsignadaParaEntidadViewModel>> BuscarPoliticasAsignadasPorEntidad(
            Guid id_Entidad
        )
    {
        return await _servicioDeDominioServiceApp.BuscarPoliticasAsignadasPorEntidad(
            id_Entidad
        );
    }

    [Authorize]
    [HttpGet("BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion")]
    public async Task<EntidadViewModel?> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion( 
            Guid id_Usuario, 
            Guid id_Organizacion 
        ) 
    {
        return await _servicioDeDominioServiceApp.BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion( 
            id_Usuario, 
            id_Organizacion 
        ); 
    } 
}

