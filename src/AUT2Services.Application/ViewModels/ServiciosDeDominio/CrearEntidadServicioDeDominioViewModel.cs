using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class CrearEntidadServicioDeDominioViewModel
{
    [DisplayName("Id_UnidadOrganizacional")]
    public Guid? Id_UnidadOrganizacional { get; set; }
    [DisplayName("Id_Usuario")]
    public Guid? Id_Usuario { get; set; }
    [DisplayName("TipoDeEntidad")] 
    public string? TipoDeEntidad { get; set; }
    [DisplayName("CorreoElectronico")]
    public string? CorreoElectronico { get; set; }
}

