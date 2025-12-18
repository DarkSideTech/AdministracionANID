using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class CrearOrganizacionServicioDeDominioViewModel
{
    [DisplayName("IdOrganizacion")]
    public string? IdOrganizacion { get; set; }
    [DisplayName("Codigo")]
    public string? Codigo { get; set; }
    [DisplayName("Nombre")]
    public string? Nombre { get; set; }
    [DisplayName("Descripcion")]
    public string? Descripcion { get; set; }
}

