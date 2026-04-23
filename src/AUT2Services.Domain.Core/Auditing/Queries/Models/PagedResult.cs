namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class PagedResult<T>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public long TotalCount { get; init; }
    public IReadOnlyCollection<T> Items { get; init; } = [];
}
