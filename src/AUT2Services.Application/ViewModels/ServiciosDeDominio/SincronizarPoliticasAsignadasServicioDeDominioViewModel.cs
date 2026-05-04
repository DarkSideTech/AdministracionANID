using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class SincronizarPoliticasAsignadasServicioDeDominioViewModel
{
    [DisplayName("Accion")]
    public string? Accion { get; set; }

    [DisplayName("Items")]
    public IEnumerable<SincronizarPoliticaAsignadaEntidadItemViewModel>? Items { get; set; } = [];
}
