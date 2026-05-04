namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public sealed record AsignacionRolPendienteValidacionViewModel(
    Guid IdPoliticaAsignada,
    Guid IdEntidad,
    Guid IdUsuario,
    string NombreUsuario,
    string CorreoElectronico,
    Guid IdOrganizacion,
    string CodigoOrganizacion,
    string NombreOrganizacion,
    Guid IdUnidadOrganizacional,
    string CodigoUnidadOrganizacional,
    string NombreUnidadOrganizacional,
    string TipoDeEntidad,
    Guid IdRol,
    string NombreRol,
    Guid IdProceso,
    string CodigoProceso,
    string NombreProceso,
    DateTimeOffset? FechaCreacion,
    DateTimeOffset? FechaInicioAsignacion,
    DateTimeOffset? FechaTerminoAsignacion,
    bool RolRequiereValidacion,
    bool RolAsignadoValidado);

public sealed record BuscarAsignacionesRolesPendientesValidacionResponse(
    int NumeroDePagina,
    int CantidadPorPagina,
    long Total,
    IReadOnlyCollection<AsignacionRolPendienteValidacionViewModel> Items);
