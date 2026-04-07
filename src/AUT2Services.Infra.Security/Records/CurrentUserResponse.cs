namespace AUT2Services.Infra.Security.Records;

public sealed record CurrentUserResponse(
    bool IsAuthenticated,
    string? UserId,
    string? Email,
    string? NombreADesplegar,
    IList<OrganizacionesPorUsuario>? OrganizacionesPorUsuario,
    IList<ProcesoActivo>? ProcesosActivos,
    DateTime? ExpiresAtUtc,
    string? OrganizacionSeleccionada,
    string? EntidadIdSeleccionada,
    bool SeleccionOrganizacionrequerida
);
