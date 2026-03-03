// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.943
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.AutenticadoresExternos;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class AutenticadorExternoController : ApiController
{
    private readonly IAutenticadorExternoServiceApp _autenticadorExternoServiceApp;
    private readonly ILogger<AutenticadorExternoController> _logger;

    public AutenticadorExternoController(IAutenticadorExternoServiceApp autenticadorExternoServiceApp, ILogger<AutenticadorExternoController> logger)
    {
        _autenticadorExternoServiceApp = autenticadorExternoServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearAutenticadorExternoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _autenticadorExternoServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarAutenticadorExternoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _autenticadorExternoServiceApp.Modificar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarAutenticadorExternoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _autenticadorExternoServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPut("MarcarComoValidadorPrimario")]
    public async Task<IActionResult> MarcarComoValidadorPrimario(MarcarComoValidadorPrimarioAutenticadorExternoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _autenticadorExternoServiceApp.MarcarComoValidadorPrimario(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarAutenticadorExternoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _autenticadorExternoServiceApp.Activar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarAutenticadorExternoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _autenticadorExternoServiceApp.Desactivar(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<AutenticadorExternoViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _autenticadorExternoServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Proveedor")]
    public async Task<IEnumerable<AutenticadorExternoViewModel>> BuscarPor_Id_Proveedor( 
            Guid id_Proveedor 
        ) 
    {
        return await _autenticadorExternoServiceApp.BuscarPor_Id_Proveedor( 
            id_Proveedor 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Usuario")]
    public async Task<IEnumerable<AutenticadorExternoViewModel>> BuscarPor_Id_Usuario( 
            Guid id_Usuario 
        ) 
    {
        return await _autenticadorExternoServiceApp.BuscarPor_Id_Usuario( 
            id_Usuario 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Usuario_ValidadorPrimario")]
    public async Task<AutenticadorExternoViewModel> BuscarPor_Id_Usuario_ValidadorPrimario( 
            Guid id_Usuario 
        ) 
    {
        return await _autenticadorExternoServiceApp.BuscarPor_Id_Usuario_ValidadorPrimario( 
            id_Usuario 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Proveedor_Id_Usuario")]
    public async Task<AutenticadorExternoViewModel> BuscarPor_Id_Proveedor_Id_Usuario( 
            Guid id_Proveedor, 
            Guid id_Usuario 
        ) 
    {
        return await _autenticadorExternoServiceApp.BuscarPor_Id_Proveedor_Id_Usuario( 
            id_Proveedor, 
            id_Usuario 
        ); 
    } 

}

