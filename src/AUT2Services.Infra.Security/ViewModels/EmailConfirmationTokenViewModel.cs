namespace AUT2Services.Infra.Security.ViewModels;

public class EmailConfirmationTokenViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string ConfirmationToken { get; set; } = string.Empty;
}
