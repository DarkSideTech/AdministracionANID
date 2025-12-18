// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.151
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Procesos;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProcesoController : ApiController
{
    private readonly IProcesoServiceApp _procesoServiceApp;
    private readonly ILogger<ProcesoController> _logger;

    public ProcesoController(IProcesoServiceApp procesoServiceApp, ILogger<ProcesoController> logger)
    {
        _procesoServiceApp = procesoServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearProcesoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _procesoServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarProcesoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _procesoServiceApp.Modificar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarProcesoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _procesoServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarProcesoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _procesoServiceApp.Activar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarProcesoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _procesoServiceApp.Desactivar(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarTodos")]
    public async Task<IEnumerable<ProcesoViewModel>> BuscarTodos() 
    {
        return await _procesoServiceApp.BuscarTodos(); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<ProcesoViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _procesoServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Codigo")]
    public async Task<ProcesoViewModel> BuscarPor_Codigo( 
            string codigo 
        ) 
    {
        return await _procesoServiceApp.BuscarPor_Codigo( 
            codigo 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_IdMacro_Proceso")]
    public async Task<IEnumerable<ProcesoViewModel>> BuscarPor_IdMacro_Proceso( 
            Guid idMacro_Proceso 
        ) 
    {
        return await _procesoServiceApp.BuscarPor_IdMacro_Proceso( 
            idMacro_Proceso 
        ); 
    } 

}

