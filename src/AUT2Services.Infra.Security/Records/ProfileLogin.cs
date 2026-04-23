using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Records;

public sealed record ProfileLogin(
    DateTimeOffset? AccessTokenExpiracion,
    IList<OrganizacionPorUsuario>? OrganizacionesPorUsuario,
    IList<UnidadOrganizacionalEntidadRolPorUsuario>? UnidadesOrganizacionalesPorUsuario,
    UserDto User,
    IList<ProcesoActivo>? ProcesosActivos,
    string? CodigoOrganizacionSeleccionada,
    string? NombreOrganizacionSeleccionada,
    string? CodigoUnidadOrganizacionalSeleccionada,
    string? NombreUnidadOrganizacionalSeleccionada,
    string? IdEntidadSeleccionada,
    EntidadRolSeleccionado? EntidadRolSeleccionado,
    bool SeleccionOrganizacionRequerida
    );

