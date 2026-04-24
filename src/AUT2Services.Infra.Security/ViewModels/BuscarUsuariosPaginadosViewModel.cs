using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class BuscarUsuariosPaginadosViewModel
{
    [DisplayName("NumeroDePagina")]
    public int? NumeroDePagina { get; set; } = 1;

    [DisplayName("CantidadPorPagina")]
    public int? CantidadPorPagina { get; set; } = 10;

    [DisplayName("Busqueda")]
    public string? Busqueda { get; set; } = string.Empty;
}
