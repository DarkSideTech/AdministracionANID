using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Records;

public sealed record CurrentUserResponse(
    bool IsAuthenticated,
    string? UserId,
    string? Email,
    string? NombreADesplegar,
    UserDto? User,
    IList<OrganizacionPorUsuario>? OrganizacionesPorUsuario,
    IList<UnidadOrganizacionalEntidadRolPorUsuario>? UnidadesOrganizacionalesPorUsuario,
    IList<ProcesoActivo>? ProcesosActivos,
    DateTimeOffset? ExpiresAtUtc,
    string? OrganizacionSeleccionada,
    string? NombreOrganizacionSeleccionada,
    string? CodigoUnidadOrganizacionalSeleccionada,
    string? NombreUnidadOrganizacionalSeleccionada,
    string? EntidadIdSeleccionada,
    EntidadRolSeleccionado? EntidadRolSeleccionado,
    bool SeleccionOrganizacionrequerida
);
