namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class TraceabilityChangeItem
{
    public int Order { get; init; }
    public string Path { get; init; } = string.Empty;
    public string ValueType { get; init; } = string.Empty;
    public string? NewValueJson { get; init; }
}
