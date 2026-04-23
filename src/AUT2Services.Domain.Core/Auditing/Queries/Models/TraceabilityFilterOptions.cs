namespace AUT2Services.Domain.Core.Auditing.Queries.Models;

public sealed class TraceabilityFilterOptions
{
    public IReadOnlyCollection<string> OperationTypes { get; init; } = [];
    public IReadOnlyCollection<string> EventTypes { get; init; } = [];
    public IReadOnlyCollection<string> CommandTypes { get; init; } = [];
}
