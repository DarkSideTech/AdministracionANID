using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class DesactivarUsuarioViewModel
{
    [DisplayName("IdUsuario")]
    public string? IdUsuario { get; set; } = string.Empty;
}
