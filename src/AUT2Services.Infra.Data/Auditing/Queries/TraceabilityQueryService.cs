using AUT2Services.Domain.Core.Auditing.Queries;
using AUT2Services.Domain.Core.Auditing.Queries.Models;

namespace AUT2Services.Infra.Data.Auditing.Queries;

public sealed class TraceabilityQueryService(
    ITraceabilityReadStore readStore,
    ITraceabilityEntityCatalog entityCatalog) : ITraceabilityQueryService
{
    public Task<IReadOnlyList<TraceabilityEntityCatalogItem>> GetEntitiesAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TraceabilityEntityCatalogItem> entities = entityCatalog
            .GetAll()
            .Where(item => item.Enabled)
            .OrderBy(item => item.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return Task.FromResult(entities);
    }

    public Task<TraceabilityFilterOptions> GetFilterOptionsAsync(
        TraceabilityFilterValuesRequest request,
        CancellationToken cancellationToken = default)
    {
        return readStore.GetFilterOptionsAsync(request, cancellationToken);
    }

    public Task<PagedResult<TraceabilityEventListItem>> SearchAsync(
        TraceabilitySearchRequest request,
        CancellationToken cancellationToken = default)
    {
        return readStore.SearchAsync(request, cancellationToken);
    }

    public Task<TraceabilityEventDetail?> GetEventAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return readStore.GetEventAsync(id, cancellationToken);
    }

    public Task<IReadOnlyList<TraceabilityEventListItem>> GetAggregateTimelineAsync(
        string? entityKey,
        Guid aggregateId,
        DateTimeOffset? fromUtc = null,
        DateTimeOffset? toUtc = null,
        CancellationToken cancellationToken = default)
    {
        return readStore.GetAggregateTimelineAsync(entityKey, aggregateId, fromUtc, toUtc, cancellationToken);
    }
}
