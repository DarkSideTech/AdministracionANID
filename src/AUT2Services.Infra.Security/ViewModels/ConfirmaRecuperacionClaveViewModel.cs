using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class ConfirmaRecuperacionClaveViewModel
{
    [DisplayName("CorreoElectronico")]
    public string? CorreoElectronico { get; set; } = string.Empty;

    [DisplayName("CodigoValidacion")]
    public string? CodigoValidacion { get; set; } = string.Empty;

    [DisplayName("NuevaClave")]
    public string? NuevaClave { get; set; } = string.Empty;

    [DisplayName("ConfirmaNuevaClave")]
    public string? ConfirmaNuevaClave { get; set; } = string.Empty;
}
