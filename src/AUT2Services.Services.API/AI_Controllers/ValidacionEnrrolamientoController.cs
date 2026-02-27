// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.340
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ValidacionEnrrolamientos;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class ValidacionEnrrolamientoController : ApiController
{
    private readonly IValidacionEnrrolamientoServiceApp _validacionEnrrolamientoServiceApp;
    private readonly ILogger<ValidacionEnrrolamientoController> _logger;

    public ValidacionEnrrolamientoController(IValidacionEnrrolamientoServiceApp validacionEnrrolamientoServiceApp, ILogger<ValidacionEnrrolamientoController> logger)
    {
        _validacionEnrrolamientoServiceApp = validacionEnrrolamientoServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ENRROLAMIENTO)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearValidacionEnrrolamientoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _validacionEnrrolamientoServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ENRROLAMIENTO)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarValidacionEnrrolamientoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _validacionEnrrolamientoServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ENRROLAMIENTO)]
    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarValidacionEnrrolamientoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _validacionEnrrolamientoServiceApp.Activar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.VALIDA_ENRROLAMIENTO)]
    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarValidacionEnrrolamientoViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _validacionEnrrolamientoServiceApp.Desactivar(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<ValidacionEnrrolamientoViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _validacionEnrrolamientoServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario")]
    public async Task<ValidacionEnrrolamientoViewModel> BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario( 
            Guid idValidado_Usuario, 
            Guid idValidaEnrrolamiento_Usuario 
        ) 
    {
        return await _validacionEnrrolamientoServiceApp.BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario( 
            idValidado_Usuario, 
            idValidaEnrrolamiento_Usuario 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_IdValidado_Usuario")]
    public async Task<IEnumerable<ValidacionEnrrolamientoViewModel>> BuscarPor_IdValidado_Usuario( 
            Guid idValidado_Usuario 
        ) 
    {
        return await _validacionEnrrolamientoServiceApp.BuscarPor_IdValidado_Usuario( 
            idValidado_Usuario 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_IdValidaEnrrolamiento_Usuario")]
    public async Task<IEnumerable<ValidacionEnrrolamientoViewModel>> BuscarPor_IdValidaEnrrolamiento_Usuario( 
            Guid idValidaEnrrolamiento_Usuario 
        ) 
    {
        return await _validacionEnrrolamientoServiceApp.BuscarPor_IdValidaEnrrolamiento_Usuario( 
            idValidaEnrrolamiento_Usuario 
        ); 
    } 

}

