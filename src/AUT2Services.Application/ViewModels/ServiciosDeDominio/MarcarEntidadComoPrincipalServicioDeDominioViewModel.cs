using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class MarcarEntidadComoPrincipalServicioDeDominioViewModel
{
    [DisplayName("Id_Entidad")] 
    public Guid? Id_Entidad { get; set; }
}