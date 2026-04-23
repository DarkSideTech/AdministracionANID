using AUT2Services.Domain.Core.Auditing.Queries.Models;

namespace AUT2Services.Domain.Core.Auditing.Queries;

public interface ITraceabilityEntityCatalog
{
    IReadOnlyList<TraceabilityEntityCatalogItem> GetAll();
    TraceabilityEntityCatalogItem? Find(string? entityKey);
}
