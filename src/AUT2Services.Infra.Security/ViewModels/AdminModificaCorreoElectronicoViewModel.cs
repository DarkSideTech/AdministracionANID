using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class AdminModificaCorreoElectronicoViewModel
{
    [DisplayName("IdUsuario")]
    public string? IdUsuario { get; set; } = string.Empty;

    [DisplayName("NuevoCorreoElectronico")]
    public string? NuevoCorreoElectronico { get; set; } = string.Empty;
}
