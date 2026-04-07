namespace AUT2Services.Infra.Security.Records;

public sealed record ProfileLogin(
    DateTime AccessTokenExpiracion,
    IList<OrganizacionesPorUsuario>? OrganizacionesPorUsuario,
    UserDto User,
    IList<ProcesoActivo>? ProcesosActivos,
    string? CodigoOrganizacionSeleccionada,
    string? IdEntidadSeleccionada,
    bool SeleccionOrganizacionRequerida
    );

