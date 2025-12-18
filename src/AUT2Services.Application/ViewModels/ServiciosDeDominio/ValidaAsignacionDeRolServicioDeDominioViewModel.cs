using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class ValidaAsignacionDeRolServicioDeDominioViewModel
{
    [DisplayName("Id_PoliticaAsignada")] 
    public Guid? Id_PoliticaAsignada { get; set; }
    public Guid? Id_Usuario_ValidaAsignacionRol { get; set; }
}