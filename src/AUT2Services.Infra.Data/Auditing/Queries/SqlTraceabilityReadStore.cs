using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Auditing.Queries;
using AUT2Services.Domain.Core.Auditing.Queries.Models;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Auditing.Queries;

public sealed class SqlTraceabilityReadStore(
    AUT2ServicesContext dbContext,
    ITraceabilityEntityCatalog entityCatalog) : ITraceabilityReadStore
{
    public TraceabilityReadProvider Provider => TraceabilityReadProvider.Sql;

    public async Task<PagedResult<TraceabilityEventListItem>> SearchAsync(
        TraceabilitySearchRequest request,
        CancellationToken cancellationToken = default)
    {
        var normalizedRequest = NormalizeRequest(request);
        var query = BuildBaseQuery(
            normalizedRequest.EntityKey,
            normalizedRequest.AggregateId,
            normalizedRequest.FromUtc,
            normalizedRequest.ToUtc,
            normalizedRequest.CorrelationId,
            normalizedRequest.ActorUserId);

        if (query is null)
        {
            return CreateEmptyPage<TraceabilityEventListItem>(normalizedRequest.Page, normalizedRequest.PageSize);
        }

        var operationTypes = ParseOperationTypes(normalizedRequest.OperationTypes);
        if (normalizedRequest.OperationTypes.Count > 0)
        {
            if (operationTypes.Count == 0)
            {
                return CreateEmptyPage<TraceabilityEventListItem>(normalizedRequest.Page, normalizedRequest.PageSize);
            }

            query = query.Where(item => operationTypes.Contains(item.OperationType));
        }

        var eventTypes = NormalizeValues(normalizedRequest.EventTypes);
        if (eventTypes.Count > 0)
        {
            query = query.Where(item => eventTypes.Contains(item.EventType));
        }

        var commandTypes = NormalizeValues(normalizedRequest.CommandTypes);
        if (commandTypes.Count > 0)
        {
            query = query.Where(item => commandTypes.Contains(item.CommandType));
        }

        var totalCount = await query.LongCountAsync(cancellationToken).ConfigureAwait(false);
        var skip = (normalizedRequest.Page - 1) * normalizedRequest.PageSize;
        var orderedQuery = ApplySort(query, normalizedRequest.SortDirection);

        var items = await orderedQuery
            .Skip(skip)
            .Take(normalizedRequest.PageSize)
            .Select(item => new TraceabilityEventListItem
            {
                Id = item.Id,
                AggregateId = item.AggregateId,
                AggregateType = item.AggregateType,
                AggregateRevision = item.AggregateRevision,
                EventType = item.EventType,
                CommandType = item.CommandType,
                OperationType = item.OperationType.ToString(),
                OccurredAtUtc = item.OccurredAtUtc,
                PersistedAtUtc = item.PersistedAtUtc,
                ActorUsername = item.ActorUsername,
                ActorEmail = item.ActorEmail,
                RequestPath = item.RequestPath,
                ChangeCount = item.Changes.Count
            })
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);

        return new PagedResult<TraceabilityEventListItem>
        {
            Page = normalizedRequest.Page,
            PageSize = normalizedRequest.PageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<TraceabilityEventDetail?> GetEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.AuditOutboxMessages
            .AsNoTracking()
            .Include(item => item.Changes)
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return message is null ? null : MapDetail(message);
    }

    public async Task<IReadOnlyList<TraceabilityEventListItem>> GetAggregateTimelineAsync(
        string? entityKey,
        Guid aggregateId,
        DateTimeOffset? fromUtc,
        DateTimeOffset? toUtc,
        CancellationToken cancellationToken = default)
    {
        var query = BuildBaseQuery(entityKey, aggregateId, fromUtc, toUtc, null, null);
        if (query is null)
        {
            return [];
        }

        return await ApplySort(query, "desc")
            .Select(item => new TraceabilityEventListItem
            {
                Id = item.Id,
                AggregateId = item.AggregateId,
                AggregateType = item.AggregateType,
                AggregateRevision = item.AggregateRevision,
                EventType = item.EventType,
                CommandType = item.CommandType,
                OperationType = item.OperationType.ToString(),
                OccurredAtUtc = item.OccurredAtUtc,
                PersistedAtUtc = item.PersistedAtUtc,
                ActorUsername = item.ActorUsername,
                ActorEmail = item.ActorEmail,
                RequestPath = item.RequestPath,
                ChangeCount = item.Changes.Count
            })
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<TraceabilityFilterOptions> GetFilterOptionsAsync(
        TraceabilityFilterValuesRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = BuildBaseQuery(request.EntityKey, request.AggregateId, request.FromUtc, request.ToUtc, null, null);
        if (query is null)
        {
            return new TraceabilityFilterOptions();
        }

        var operationTypes = await query
            .Select(item => item.OperationType)
            .Distinct()
            .OrderBy(item => item)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var eventTypes = await query
            .Select(item => item.EventType)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct()
            .OrderBy(item => item)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var commandTypes = await query
            .Select(item => item.CommandType)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct()
            .OrderBy(item => item)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new TraceabilityFilterOptions
        {
            OperationTypes = operationTypes.Select(item => item.ToString()).ToArray(),
            EventTypes = eventTypes,
            CommandTypes = commandTypes
        };
    }

    private IQueryable<AuditOutboxMessage>? BuildBaseQuery(
        string? entityKey,
        Guid? aggregateId,
        DateTimeOffset? fromUtc,
        DateTimeOffset? toUtc,
        Guid? correlationId,
        string? actorUserId)
    {
        var query = dbContext.AuditOutboxMessages
            .AsNoTracking()
            .AsQueryable();

        var aggregateType = ResolveAggregateType(entityKey);
        if (entityKey is not null && aggregateType is null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(aggregateType))
        {
            query = query.Where(item => item.AggregateType == aggregateType);
        }

        if (aggregateId.HasValue)
        {
            query = query.Where(item => item.AggregateId == aggregateId.Value);
        }

        if (fromUtc.HasValue)
        {
            query = query.Where(item => item.OccurredAtUtc >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            query = query.Where(item => item.OccurredAtUtc <= toUtc.Value);
        }

        if (correlationId.HasValue)
        {
            query = query.Where(item => item.CorrelationId == correlationId.Value);
        }

        if (!string.IsNullOrWhiteSpace(actorUserId))
        {
            query = query.Where(item => item.ActorUserId == actorUserId.Trim());
        }

        return query;
    }

    private string? ResolveAggregateType(string? entityKey)
    {
        if (string.IsNullOrWhiteSpace(entityKey))
        {
            return null;
        }

        return entityCatalog.Find(entityKey)?.AggregateType;
    }

    private static IQueryable<AuditOutboxMessage> ApplySort(
        IQueryable<AuditOutboxMessage> query,
        string? sortDirection)
    {
        return string.Equals(sortDirection?.Trim(), "asc", StringComparison.OrdinalIgnoreCase)
            ? query.OrderBy(item => item.AggregateRevision)
                .ThenBy(item => item.OccurredAtUtc)
                .ThenBy(item => item.Id)
            : query.OrderByDescending(item => item.AggregateRevision)
                .ThenByDescending(item => item.OccurredAtUtc)
                .ThenByDescending(item => item.Id);
    }

    private static TraceabilityEventDetail MapDetail(AuditOutboxMessage item)
    {
        return new TraceabilityEventDetail
        {
            Id = item.Id,
            CorrelationId = item.CorrelationId,
            AggregateId = item.AggregateId,
            AggregateType = item.AggregateType,
            AggregateRevision = item.AggregateRevision,
            EventType = item.EventType,
            CommandType = item.CommandType,
            OperationType = item.OperationType.ToString(),
            OccurredAtUtc = item.OccurredAtUtc,
            PersistedAtUtc = item.PersistedAtUtc,
            RequestPath = item.RequestPath,
            SnapshotJson = item.SnapshotJson,
            Actor = new TraceabilityActorDto
            {
                UserId = item.ActorUserId,
                Username = item.ActorUsername,
                Email = item.ActorEmail
            },
            Changes = item.Changes
                .OrderBy(change => change.Order)
                .Select(change => new TraceabilityChangeItem
                {
                    Order = change.Order,
                    Path = change.Path,
                    ValueType = change.ValueType ?? string.Empty,
                    NewValueJson = change.NewValueJson
                })
                .ToArray()
        };
    }

    private static TraceabilitySearchRequest NormalizeRequest(TraceabilitySearchRequest request)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize switch
        {
            <= 0 => 50,
            > 200 => 200,
            _ => request.PageSize
        };

        return new TraceabilitySearchRequest
        {
            EntityKey = Normalize(request.EntityKey),
            AggregateId = request.AggregateId,
            FromUtc = request.FromUtc,
            ToUtc = request.ToUtc,
            OperationTypes = NormalizeValues(request.OperationTypes),
            EventTypes = NormalizeValues(request.EventTypes),
            CommandTypes = NormalizeValues(request.CommandTypes),
            CorrelationId = request.CorrelationId,
            ActorUserId = Normalize(request.ActorUserId),
            Page = page,
            PageSize = pageSize,
            SortDirection = Normalize(request.SortDirection) ?? "desc"
        };
    }

    private static List<string> NormalizeValues(IEnumerable<string>? values)
    {
        return values?
            .Select(Normalize)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Cast<string>()
            .ToList()
            ?? [];
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static List<AuditOperationType> ParseOperationTypes(IEnumerable<string>? values)
    {
        var result = new List<AuditOperationType>();

        foreach (var value in values ?? [])
        {
            var normalizedValue = Normalize(value);
            if (normalizedValue is null)
            {
                continue;
            }

            if (Enum.TryParse<AuditOperationType>(normalizedValue, true, out var parsedValue)
                && !result.Contains(parsedValue))
            {
                result.Add(parsedValue);
            }
        }

        return result;
    }

    private static PagedResult<T> CreateEmptyPage<T>(int page, int pageSize)
    {
        return new PagedResult<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = 0,
            Items = []
        };
    }
}
