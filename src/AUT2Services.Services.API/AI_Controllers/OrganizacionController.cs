// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.342
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Organizaciones;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class OrganizacionController : ApiController
{
    private readonly IOrganizacionServiceApp _organizacionServiceApp;
    private readonly ILogger<OrganizacionController> _logger;

    public OrganizacionController(IOrganizacionServiceApp organizacionServiceApp, ILogger<OrganizacionController> logger)
    {
        _organizacionServiceApp = organizacionServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearOrganizacionViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _organizacionServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarOrganizacionViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _organizacionServiceApp.Modificar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarOrganizacionViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _organizacionServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpDelete("EliminarPor_Codigo")]
    public async Task<IActionResult> EliminarPor_Codigo(EliminarPor_CodigoOrganizacionViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _organizacionServiceApp.EliminarPor_Codigo(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarOrganizacionViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _organizacionServiceApp.Activar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD)]
    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarOrganizacionViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _organizacionServiceApp.Desactivar(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarTodos")]
    public async Task<IEnumerable<OrganizacionViewModel>> BuscarTodos() 
    {
        return await _organizacionServiceApp.BuscarTodos(); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<OrganizacionViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _organizacionServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Codigo")]
    public async Task<OrganizacionViewModel> BuscarPor_Codigo( 
            string codigo 
        ) 
    {
        return await _organizacionServiceApp.BuscarPor_Codigo( 
            codigo 
        ); 
    } 

}

