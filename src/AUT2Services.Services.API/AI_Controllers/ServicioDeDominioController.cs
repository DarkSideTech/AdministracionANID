// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.151
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ServicioDeDominioController : ApiController
{
    private readonly IServicioDeDominioServiceApp _servicioDeDominioServiceApp;
    private readonly ILogger<ServicioDeDominioController> _logger;

    public ServicioDeDominioController(IServicioDeDominioServiceApp servicioDeDominioServiceApp, ILogger<ServicioDeDominioController> logger)
    {
        _servicioDeDominioServiceApp = servicioDeDominioServiceApp;
        _logger = logger;
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

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpPost("CrearEntidad")]
    public async Task<IActionResult> CrearEntidad(CrearEntidadServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.CrearEntidad(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD_USUARIO)]
    [HttpPost("MarcarEntidadComoPrincipal")]
    public async Task<IActionResult> MarcarEntidadComoPrincipal(MarcarEntidadComoPrincipalServicioDeDominioViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _servicioDeDominioServiceApp.MarcarEntidadComoPrincipal(dataViewModel));
    } 

    [AllowAnonymous]
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

    [AllowAnonymous]
    [HttpGet("BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion")]
    public async Task<EntidadViewModel> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion( 
            Guid id_Usuario, 
            Guid id_Organizacion 
        ) 
    {
        return await _servicioDeDominioServiceApp.BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion( 
            id_Usuario, 
            id_Organizacion 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarOrganizacionesPor_Id_Usuario")]
    public async Task<IEnumerable<OrganizacionPorUsuarioViewModel>> BuscarOrganizacionesPor_Id_Usuario( 
            Guid id_Usuario 
        ) 
    {
        return await _servicioDeDominioServiceApp.BuscarOrganizacionesPor_Id_Usuario( 
            id_Usuario 
        ); 
    } 

}

