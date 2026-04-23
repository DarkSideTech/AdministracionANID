using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class ReenviaCodigoCambioClaveViewModel
{
    [DisplayName("IdUsuario")]
    public string? IdUsuario { get; set; } = string.Empty;
}
