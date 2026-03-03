// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.941
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Proveedores;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class ProveedorController : ApiController
{
    private readonly IProveedorServiceApp _proveedorServiceApp;
    private readonly ILogger<ProveedorController> _logger;

    public ProveedorController(IProveedorServiceApp proveedorServiceApp, ILogger<ProveedorController> logger)
    {
        _proveedorServiceApp = proveedorServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Modificar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpDelete("EliminarPor_Codigo")]
    public async Task<IActionResult> EliminarPor_Codigo(EliminarPor_CodigoProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.EliminarPor_Codigo(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Activar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Desactivar(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarTodos")]
    public async Task<IEnumerable<ProveedorViewModel>> BuscarTodos() 
    {
        return await _proveedorServiceApp.BuscarTodos(); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<ProveedorViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _proveedorServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Codigo")]
    public async Task<ProveedorViewModel> BuscarPor_Codigo( 
            string codigo 
        ) 
    {
        return await _proveedorServiceApp.BuscarPor_Codigo( 
            codigo 
        ); 
    } 

}

