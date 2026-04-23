using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class SolicitaCambioClaveViewModel
{
    [DisplayName("IdUsuario")]
    public string? IdUsuario { get; set; } = string.Empty;

    [DisplayName("ClaveActual")]
    public string? ClaveActual { get; set; } = string.Empty;

    [DisplayName("NuevaClave")]
    public string? NuevaClave { get; set; } = string.Empty;

    [DisplayName("ConfirmaNuevaClave")]
    public string? ConfirmaNuevaClave { get; set; } = string.Empty;
}
