namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class TraceabilitySearchRequest
{
    public string? EntityKey { get; init; }
    public Guid? AggregateId { get; init; }
    public DateTimeOffset? FromUtc { get; init; }
    public DateTimeOffset? ToUtc { get; init; }
    public IReadOnlyCollection<string> OperationTypes { get; init; } = [];
    public IReadOnlyCollection<string> EventTypes { get; init; } = [];
    public IReadOnlyCollection<string> CommandTypes { get; init; } = [];
    public Guid? CorrelationId { get; init; }
    public string? ActorUserId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public string SortDirection { get; init; } = "desc";
}
