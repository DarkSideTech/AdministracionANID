// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.454
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Entidades;
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class EntidadController : ApiController
{
    private readonly IEntidadServiceApp _entidadServiceApp;
    private readonly IAuditJournalReader _auditJournalReader;
    private readonly ILogger<EntidadController> _logger;

    public EntidadController(
        IEntidadServiceApp entidadServiceApp,
        IAuditJournalReader auditJournalReader,
        ILogger<EntidadController> logger)
    {
        _entidadServiceApp = entidadServiceApp;
        _auditJournalReader = auditJournalReader;
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

    [Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
    [HttpGet("BuscarPor_Id_Usuario_Id_Organizacion")]
    public async Task<IEnumerable<EntidadViewModel>> BuscarPor_Id_Usuario_Id_Organizacion(
            Guid id_Usuario,
            Guid id_Organizacion
        )
    {
        return await _entidadServiceApp.BuscarPor_Id_Usuario_Id_Organizacion(
            id_Usuario,
            id_Organizacion
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

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpGet("BuscarTrazabilidadPor_Id")]
    public async Task<ActionResult<IReadOnlyList<AuditEnvelope>>> BuscarTrazabilidadPor_Id(Guid id)
    {
        var timeline = await _auditJournalReader.GetAggregateTimelineAsync(id);
        return Ok(OrderAuditTimelineDescending(timeline));
    }

}

