using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class SolicitaRecuperacionClaveViewModel
{
    [DisplayName("CorreoElectronico")]
    public string? CorreoElectronico { get; set; } = string.Empty;
}
