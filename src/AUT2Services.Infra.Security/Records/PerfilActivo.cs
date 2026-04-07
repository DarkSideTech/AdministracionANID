namespace AUT2Services.Infra.Security.Records;

public sealed record PerfilActivo (
    string? Codigo_UnidadOrganizacional,
    string? Nombre_UnidadOrganizacional,
    Guid? Id_Entidad,
    IList<ProcesoActivo>? ProcesosActivos
);