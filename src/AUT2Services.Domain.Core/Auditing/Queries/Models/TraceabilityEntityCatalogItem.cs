namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class TraceabilityEntityCatalogItem
{
    public string EntityKey { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string AggregateType { get; init; } = string.Empty;
    public bool Enabled { get; init; } = true;
}
