namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class TraceabilityFilterValuesRequest
{
    public string? EntityKey { get; init; }
    public Guid? AggregateId { get; init; }
    public DateTimeOffset? FromUtc { get; init; }
    public DateTimeOffset? ToUtc { get; init; }
}
