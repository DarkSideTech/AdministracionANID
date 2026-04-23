using AUT2Services.Domain.Core.Auditing.Queries;
using AUT2Services.Domain.Core.Auditing.Queries.Models;
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
[Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD)]
public sealed class TraceabilityController(
    ITraceabilityQueryService traceabilityQueryService) : ApiController
{
    [HttpGet("entidades")]
    public async Task<ActionResult<IReadOnlyList<TraceabilityEntityCatalogItem>>> BuscarEntidades(CancellationToken cancellationToken)
    {
        var entities = await traceabilityQueryService.GetEntitiesAsync(cancellationToken);
        return Ok(entities);
    }

    [HttpPost("filtros")]
    public async Task<ActionResult<TraceabilityFilterOptions>> BuscarFiltros(
        [FromBody] TraceabilityFilterValuesRequest? request,
        CancellationToken cancellationToken)
    {
        var result = await traceabilityQueryService.GetFilterOptionsAsync(
            request ?? new TraceabilityFilterValuesRequest(),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("buscar")]
    public async Task<ActionResult<PagedResult<TraceabilityEventListItem>>> Buscar(
        [FromBody] TraceabilitySearchRequest? request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            AddError("La consulta de trazabilidad es obligatoria.");
            return CustomResponse();
        }

        if (request.Page < 0)
        {
            AddError("El numero de pagina no puede ser negativo.");
            return CustomResponse();
        }

        if (request.PageSize < 0)
        {
            AddError("El tamaño de pagina no puede ser negativo.");
            return CustomResponse();
        }

        var result = await traceabilityQueryService.SearchAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("aggregate/{aggregateId:guid}")]
    public async Task<ActionResult<IReadOnlyList<TraceabilityEventListItem>>> BuscarTimelinePorAggregateId(
        Guid aggregateId,
        [FromQuery] string? entityKey,
        [FromQuery] DateTimeOffset? fromUtc,
        [FromQuery] DateTimeOffset? toUtc,
        CancellationToken cancellationToken)
    {
        var timeline = await traceabilityQueryService.GetAggregateTimelineAsync(
            entityKey,
            aggregateId,
            fromUtc,
            toUtc,
            cancellationToken);

        return Ok(timeline);
    }

    [HttpGet("eventos/{id:guid}")]
    public async Task<ActionResult<TraceabilityEventDetail>> BuscarEventoPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var detail = await traceabilityQueryService.GetEventAsync(id, cancellationToken);
        if (detail is null)
        {
            return NotFound();
        }

        return Ok(detail);
    }
}
