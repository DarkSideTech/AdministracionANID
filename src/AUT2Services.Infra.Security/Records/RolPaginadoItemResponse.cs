namespace AUT2Services.Infra.Security.Records;

public sealed record RolPaginadoItemResponse(
    string? Id,
    string? Nombre,
    string? NombreNormalizado,
    string? Descripcion,
    bool? ActivaDetalleDeAutorizaciones,
    bool? RequiereValidacionDeAsignacion,
    bool? ValidaAsignacionDeRoles,
    bool? ValidaEnrrolamiento,
    bool? RolBase,
    bool? Activo
);
