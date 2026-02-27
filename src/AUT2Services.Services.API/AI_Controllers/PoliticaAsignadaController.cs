// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.343
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.PoliticasAsignadas;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class PoliticaAsignadaController : ApiController
{
    private readonly IPoliticaAsignadaServiceApp _politicaAsignadaServiceApp;
    private readonly ILogger<PoliticaAsignadaController> _logger;

    public PoliticaAsignadaController(IPoliticaAsignadaServiceApp politicaAsignadaServiceApp, ILogger<PoliticaAsignadaController> logger)
    {
        _politicaAsignadaServiceApp = politicaAsignadaServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearPoliticaAsignadaViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _politicaAsignadaServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarPoliticaAsignadaViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _politicaAsignadaServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("FinalizaAsignacion")]
    public async Task<IActionResult> FinalizaAsignacion(FinalizaAsignacionPoliticaAsignadaViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _politicaAsignadaServiceApp.FinalizaAsignacion(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("ValidaAsignacionDeRol")]
    public async Task<IActionResult> ValidaAsignacionDeRol(ValidaAsignacionDeRolPoliticaAsignadaViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _politicaAsignadaServiceApp.ValidaAsignacionDeRol(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<PoliticaAsignadaViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _politicaAsignadaServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Entidad_Id_Rol_Id_Proceso")]
    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Entidad_Id_Rol_Id_Proceso( 
            Guid id_Entidad, 
            Guid id_Rol, 
            Guid id_Proceso 
        ) 
    {
        return await _politicaAsignadaServiceApp.BuscarPor_Id_Entidad_Id_Rol_Id_Proceso( 
            id_Entidad, 
            id_Rol, 
            id_Proceso 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Entidad")]
    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Entidad( 
            Guid id_Entidad 
        ) 
    {
        return await _politicaAsignadaServiceApp.BuscarPor_Id_Entidad( 
            id_Entidad 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Rol")]
    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Rol( 
            Guid id_Rol 
        ) 
    {
        return await _politicaAsignadaServiceApp.BuscarPor_Id_Rol( 
            id_Rol 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Proceso")]
    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_Id_Proceso( 
            Guid id_Proceso 
        ) 
    {
        return await _politicaAsignadaServiceApp.BuscarPor_Id_Proceso( 
            id_Proceso 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_RolRequiereValidacion")]
    public async Task<IEnumerable<PoliticaAsignadaViewModel>> BuscarPor_RolRequiereValidacion() 
    {
        return await _politicaAsignadaServiceApp.BuscarPor_RolRequiereValidacion(); 
    } 

}

