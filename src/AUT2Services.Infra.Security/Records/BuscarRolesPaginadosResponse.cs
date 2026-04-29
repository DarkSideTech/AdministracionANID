namespace AUT2Services.Infra.Security.Records;

public sealed record BuscarRolesPaginadosResponse(
    int NumeroDePagina,
    int CantidadPorPagina,
    long Total,
    IReadOnlyCollection<RolPaginadoItemResponse> Items
);
