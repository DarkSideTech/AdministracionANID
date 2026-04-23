using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class LoginClaveUnicaViewModel
{
    [DisplayName("ClientId")]
    public string? ClientId { get; set; } = string.Empty;

    [DisplayName("RedirectUri")]
    public string? RedirectUri { get; set; } = string.Empty;

    [DisplayName("Code")]
    public string? Code { get; set; } = string.Empty;

    [DisplayName("State")]
    public string? State { get; set; } = string.Empty;
}
