namespace AUT2Services.Infra.Security.Records;

public sealed record UnidadOrganizacionalEntidadRolPorUsuario(
    string? Codigo_UnidadOrganizacional,
    string? Nombre_UnidadOrganizacional,
    string? Id_Entidad,
    string? Id_Rol,
    string? Nombre_Rol
);
