using AUT2Services.Domain.Core.Auditing.Queries;
using AUT2Services.Domain.Core.Auditing.Queries.Models;

namespace AUT2Services.Infra.Data.Auditing.Queries;

public sealed class StaticTraceabilityEntityCatalog : ITraceabilityEntityCatalog
{
    private static readonly IReadOnlyList<TraceabilityEntityCatalogItem> Items =
    [
        new() { EntityKey = "entidad", DisplayName = "Entidad", AggregateType = "Entidad", Enabled = true },
        new() { EntityKey = "organizacion", DisplayName = "Organización", AggregateType = "Organizacion", Enabled = true },
        new() { EntityKey = "unidad-organizacional", DisplayName = "Unidad Organizacional", AggregateType = "UnidadOrganizacional", Enabled = true },
        new() { EntityKey = "proveedor", DisplayName = "Proveedor", AggregateType = "Proveedor", Enabled = true },
        new() { EntityKey = "proceso", DisplayName = "Proceso", AggregateType = "Proceso", Enabled = true },
        new() { EntityKey = "politica-asignada", DisplayName = "Política Asignada", AggregateType = "PoliticaAsignada", Enabled = true },
        new() { EntityKey = "validacion-enrrolamiento", DisplayName = "Validación Enrrolamiento", AggregateType = "ValidacionEnrrolamiento", Enabled = true },
        new() { EntityKey = "autenticador-externo", DisplayName = "Autenticador Externo", AggregateType = "AutenticadorExterno", Enabled = true },
        new() { EntityKey = "usuario", DisplayName = "Usuario", AggregateType = "UsuarioTraceabilityState", Enabled = true },
    ];

    public IReadOnlyList<TraceabilityEntityCatalogItem> GetAll()
    {
        return Items;
    }

    public TraceabilityEntityCatalogItem? Find(string? entityKey)
    {
        var normalizedEntityKey = Normalize(entityKey);
        if (normalizedEntityKey is null)
        {
            return null;
        }

        return Items.FirstOrDefault(item =>
            string.Equals(item.EntityKey, normalizedEntityKey, StringComparison.OrdinalIgnoreCase));
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
