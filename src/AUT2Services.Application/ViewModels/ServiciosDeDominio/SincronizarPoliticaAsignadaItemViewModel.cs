using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class SincronizarPoliticaAsignadaItemViewModel
{
    [DisplayName("IdRol")]
    public Guid? IdRol { get; set; }

    [DisplayName("IdProceso")]
    public Guid? IdProceso { get; set; }

    [DisplayName("RolRequiereValidacion")]
    public bool? RolRequiereValidacion { get; set; } = false;
}
