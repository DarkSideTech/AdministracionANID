namespace AUT2Services.Infra.Security.Models;

public sealed record SelectedOrganizationSession(string? OrganizationCode, Guid? IdRol);

public static class SelectedOrganizationSessionSerializer
{
    private const string Separator = "::";

    public static string? Serialize(string? organizationCode, Guid? idRol = null)
    {
        var normalizedOrganizationCode = Normalize(organizationCode);
        if (string.IsNullOrWhiteSpace(normalizedOrganizationCode))
        {
            return null;
        }

        return idRol.HasValue
            ? $"{normalizedOrganizationCode}{Separator}{idRol.Value:D}"
            : normalizedOrganizationCode;
    }

    public static SelectedOrganizationSession Parse(string? value)
    {
        var normalizedValue = Normalize(value);
        if (string.IsNullOrWhiteSpace(normalizedValue))
        {
            return new SelectedOrganizationSession(null, null);
        }

        var parts = normalizedValue.Split(Separator, 2, StringSplitOptions.None);
        var organizationCode = Normalize(parts[0]);
        if (parts.Length < 2 || !Guid.TryParse(parts[1], out var parsedRoleId))
        {
            return new SelectedOrganizationSession(organizationCode, null);
        }

        return new SelectedOrganizationSession(organizationCode, parsedRoleId);
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
