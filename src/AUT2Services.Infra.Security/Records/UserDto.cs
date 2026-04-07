namespace AUT2Services.Infra.Security.Records;

public sealed record UserDto(
    string? Id,
    string? Email,
    string? NombreADesplegar,
    IList<string>? Roles,
    IList<string>? UnidadesOrganizacionales
);
