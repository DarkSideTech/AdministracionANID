using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class BuscarUsuariosPendientesEnrrolamientoServicioDeDominioViewModel
{
    [DisplayName("NumeroDePagina")]
    public int? NumeroDePagina { get; set; } = 1;

    [DisplayName("CantidadPorPagina")]
    public int? CantidadPorPagina { get; set; } = 10;

    [DisplayName("Busqueda")]
    public string? Busqueda { get; set; } = string.Empty;
}
