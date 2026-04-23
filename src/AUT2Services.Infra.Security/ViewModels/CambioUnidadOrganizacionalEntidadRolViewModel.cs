using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class CambioUnidadOrganizacionalEntidadRolViewModel
{
    [DisplayName("Id_Entidad")]
    public string? Id_Entidad { get; set; }

    [DisplayName("Id_Rol")]
    public string? Id_Rol { get; set; }
}
