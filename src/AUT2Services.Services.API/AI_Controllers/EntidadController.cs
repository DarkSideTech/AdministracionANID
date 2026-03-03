// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.944
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Entidades;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class EntidadController : ApiController
{
    private readonly IEntidadServiceApp _entidadServiceApp;
    private readonly ILogger<EntidadController> _logger;

    public EntidadController(IEntidadServiceApp entidadServiceApp, ILogger<EntidadController> logger)
    {
        _entidadServiceApp = entidadServiceApp;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarEntidadViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _entidadServiceApp.Modificar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarEntidadViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _entidadServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("FinalizaAutorizacion")]
    public async Task<IActionResult> FinalizaAutorizacion(FinalizaAutorizacionEntidadViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _entidadServiceApp.FinalizaAutorizacion(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<EntidadViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _entidadServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal")]
    public async Task<EntidadViewModel> BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal( 
            Guid id_Usuario, 
            Guid id_UnidadOrganizacional 
        ) 
    {
        return await _entidadServiceApp.BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal( 
            id_Usuario, 
            id_UnidadOrganizacional 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Usuario_Id_UnidadOrganizacional")]
    public async Task<EntidadViewModel> BuscarPor_Id_Usuario_Id_UnidadOrganizacional( 
            Guid id_Usuario, 
            Guid id_UnidadOrganizacional 
        ) 
    {
        return await _entidadServiceApp.BuscarPor_Id_Usuario_Id_UnidadOrganizacional( 
            id_Usuario, 
            id_UnidadOrganizacional 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Usuario")]
    public async Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_Usuario( 
            Guid id_Usuario 
        ) 
    {
        return await _entidadServiceApp.BuscarPor_Id_Usuario( 
            id_Usuario 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_UnidadOrganizacional")]
    public async Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_UnidadOrganizacional( 
            Guid id_UnidadOrganizacional 
        ) 
    {
        return await _entidadServiceApp.BuscarPor_Id_UnidadOrganizacional( 
            id_UnidadOrganizacional 
        ); 
    } 

}

