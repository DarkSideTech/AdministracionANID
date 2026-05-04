namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public sealed record BuscarUsuariosPendientesEnrrolamientoResponse(
    int NumeroDePagina,
    int CantidadPorPagina,
    long Total,
    IReadOnlyCollection<UsuarioPendienteEnrrolamientoViewModel> Items);
