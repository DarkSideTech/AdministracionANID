using AUT2Services.Domain.Core.Auditing;
using System.Reflection;

namespace AUT2Services.Infra.DataTrazabilidad.Auditing;

public sealed class DefaultAuditDeltaBuilder(IAuditSerializer serializer) : IAuditDeltaBuilder
{
    private static readonly StringComparer PathComparer = StringComparer.Ordinal;

    public IReadOnlyCollection<AuditDeltaChange> Build(object? before, object? after)
    {
        if (after is null)
        {
            return [];
        }

        var afterProperties = GetComparableProperties(after.GetType());
        var beforeProperties = before is null
            ? []
            : GetComparableProperties(before.GetType()).ToDictionary(property => property.Name, PathComparer);

        List<AuditDeltaChange> changes = [];
        var order = 0;

        foreach (var afterProperty in afterProperties)
        {
            beforeProperties.TryGetValue(afterProperty.Name, out var beforeProperty);

            var oldValue = beforeProperty is null || before is null
                ? null
                : beforeProperty.GetValue(before);
            var newValue = afterProperty.GetValue(after);

            if (before is not null && ValuesAreEqual(oldValue, newValue))
            {
                continue;
            }

            changes.Add(new AuditDeltaChange(
                order++,
                afterProperty.Name,
                afterProperty.PropertyType.FullName ?? afterProperty.PropertyType.Name,
                serializer.Serialize(newValue)));
        }

        return changes;
    }

    private static IEnumerable<PropertyInfo> GetComparableProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property =>
                property.CanRead
                && property.GetIndexParameters().Length == 0
                && property.Name != "DomainEvents");
    }

    private static bool ValuesAreEqual(object? oldValue, object? newValue)
    {
        if (oldValue is null && newValue is null)
        {
            return true;
        }

        if (oldValue is null || newValue is null)
        {
            return false;
        }

        return Equals(oldValue, newValue);
    }
}
