using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class SincronizarPoliticaAsignadaEntidadItemViewModel
{
    [DisplayName("IdEntidad")]
    public Guid? IdEntidad { get; set; }

    [DisplayName("Politicas")]
    public IEnumerable<SincronizarPoliticaAsignadaItemViewModel>? Politicas { get; set; } = [];
}
