using AUT2Services.Domain.Core.Auditing.Queries.Models;

namespace AUT2Services.Domain.Core.Auditing.Queries;

public interface ITraceabilityReadStore
{
    TraceabilityReadProvider Provider { get; }

    Task<PagedResult<TraceabilityEventListItem>> SearchAsync(
        TraceabilitySearchRequest request,
        CancellationToken cancellationToken = default);

    Task<TraceabilityEventDetail?> GetEventAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TraceabilityEventListItem>> GetAggregateTimelineAsync(
        string? entityKey,
        Guid aggregateId,
        DateTimeOffset? fromUtc,
        DateTimeOffset? toUtc,
        CancellationToken cancellationToken = default);

    Task<TraceabilityFilterOptions> GetFilterOptionsAsync(
        TraceabilityFilterValuesRequest request,
        CancellationToken cancellationToken = default);
}
