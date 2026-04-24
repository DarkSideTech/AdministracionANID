namespace AUT2Services.Infra.Security.Records;

public sealed record BuscarUsuariosPaginadosResponse(
    int NumeroDePagina,
    int CantidadPorPagina,
    long Total,
    IReadOnlyCollection<UsuarioPaginadoItemResponse> Items
);
