// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.453
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.UnidadesOrganizacionales;
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class UnidadOrganizacionalController : ApiController
{
    private readonly IUnidadOrganizacionalServiceApp _unidadOrganizacionalServiceApp;
    private readonly IAuditJournalReader _auditJournalReader;
    private readonly ILogger<UnidadOrganizacionalController> _logger;

    public UnidadOrganizacionalController(
        IUnidadOrganizacionalServiceApp unidadOrganizacionalServiceApp,
        IAuditJournalReader auditJournalReader,
        ILogger<UnidadOrganizacionalController> logger)
    {
        _unidadOrganizacionalServiceApp = unidadOrganizacionalServiceApp;
        _auditJournalReader = auditJournalReader;
        _logger = logger;
    }

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearUnidadOrganizacionalViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _unidadOrganizacionalServiceApp.Crear(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarUnidadOrganizacionalViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _unidadOrganizacionalServiceApp.Modificar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarUnidadOrganizacionalViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _unidadOrganizacionalServiceApp.Eliminar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpDelete("EliminarPor_Codigo_Id_Organizacion")]
    public async Task<IActionResult> EliminarPor_Codigo_Id_Organizacion(EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _unidadOrganizacionalServiceApp.EliminarPor_Codigo_Id_Organizacion(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarUnidadOrganizacionalViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _unidadOrganizacionalServiceApp.Activar(dataViewModel));
    } 

    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarUnidadOrganizacionalViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _unidadOrganizacionalServiceApp.Desactivar(dataViewModel));
    } 

    [AllowAnonymous]
    [HttpGet("BuscarTodos")]
    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarTodos() 
    {
        return await _unidadOrganizacionalServiceApp.BuscarTodos(); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id")]
    public async Task<UnidadOrganizacionalViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _unidadOrganizacionalServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Codigo_Id_Organizacion")]
    public async Task<UnidadOrganizacionalViewModel> BuscarPor_Codigo_Id_Organizacion( 
            string codigo, 
            Guid id_Organizacion 
        ) 
    {
        return await _unidadOrganizacionalServiceApp.BuscarPor_Codigo_Id_Organizacion( 
            codigo, 
            id_Organizacion 
        ); 
    } 

    [AllowAnonymous]
    [HttpGet("BuscarPor_Id_Organizacion")]
    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarPor_Id_Organizacion( 
            Guid id_Organizacion 
        ) 
    {
        return await _unidadOrganizacionalServiceApp.BuscarPor_Id_Organizacion( 
            id_Organizacion 
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

