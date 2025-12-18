using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class LoginViewModel
{
    [DisplayName("Email")]
    public string? Email { get; set; }

    [DisplayName("Password")]
    public string? Password { get; set; }
}
