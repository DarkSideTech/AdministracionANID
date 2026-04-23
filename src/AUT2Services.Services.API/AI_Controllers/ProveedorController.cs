// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.451
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Proveedores;
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Authorize(Policy = EnumPolicyMaster.USUARIO_LOGUEADO)]
[Route("api/[controller]")]
public class ProveedorController : ApiController
{
    private readonly IProveedorServiceApp _proveedorServiceApp;
    private readonly IAuditJournalReader _auditJournalReader;
    private readonly ILogger<ProveedorController> _logger;

    public ProveedorController(
        IProveedorServiceApp proveedorServiceApp,
        IAuditJournalReader auditJournalReader,
        ILogger<ProveedorController> logger)
    {
        _proveedorServiceApp = proveedorServiceApp;
        _auditJournalReader = auditJournalReader;
        _logger = logger;
    }

    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(CrearProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Crear(dataViewModel));
    } 

    [HttpPut("Modificar")]
    public async Task<IActionResult> Modificar(ModificarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Modificar(dataViewModel));
    } 

    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar(EliminarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Eliminar(dataViewModel));
    } 

    [HttpDelete("EliminarPor_Codigo")]
    public async Task<IActionResult> EliminarPor_Codigo(EliminarPor_CodigoProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.EliminarPor_Codigo(dataViewModel));
    } 

    [HttpPut("Activar")]
    public async Task<IActionResult> Activar(ActivarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Activar(dataViewModel));
    } 

    [HttpPut("Desactivar")]
    public async Task<IActionResult> Desactivar(DesactivarProveedorViewModel dataViewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await _proveedorServiceApp.Desactivar(dataViewModel));
    } 

    [HttpGet("BuscarTodos")]
    public async Task<IEnumerable<ProveedorViewModel>> BuscarTodos() 
    {
        return await _proveedorServiceApp.BuscarTodos(); 
    } 

    [HttpGet("BuscarPor_Id")]
    public async Task<ProveedorViewModel> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await _proveedorServiceApp.BuscarPor_Id( 
            id 
        ); 
    } 

    [HttpGet("BuscarPor_Codigo")]
    public async Task<ProveedorViewModel> BuscarPor_Codigo( 
            string codigo 
        ) 
    {
        return await _proveedorServiceApp.BuscarPor_Codigo( 
            codigo 
        ); 
    } 

    [HttpGet("BuscarTrazabilidadPor_Id")]
    public async Task<ActionResult<IReadOnlyList<AuditEnvelope>>> BuscarTrazabilidadPor_Id(Guid id)
    {
        var timeline = await _auditJournalReader.GetAggregateTimelineAsync(id);
        return Ok(OrderAuditTimelineDescending(timeline));
    }

}

