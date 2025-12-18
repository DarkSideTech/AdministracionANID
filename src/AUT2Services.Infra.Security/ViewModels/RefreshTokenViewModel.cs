using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class RefreshTokenViewModel
{
    [DisplayName("RefreshToken")]
    public string? RefreshToken { get; set; } = string.Empty;

}
