using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class OrganizacionPorUsuarioViewModel
{
    [DisplayName("Codigo_Organizacion")]
    public string Codigo_Organizacion { get; set; } = string.Empty;

    [DisplayName("Nombre_Organizacion")]
    public string Nombre_Organizacion { get; set; } = string.Empty;
}
