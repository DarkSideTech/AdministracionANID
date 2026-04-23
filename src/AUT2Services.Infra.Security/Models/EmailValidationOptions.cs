namespace AUT2Services.Infra.Security.Models;

public sealed class EmailValidationOptions
{
    public string ConfirmationUrlBase { get; set; } = "http://localhost:4200/confirm-email";
    public bool ExposeConfirmationUrlInDevelopment { get; set; } = true;
    public int ResendCooldownMinutes { get; set; } = 2;
    public int ResendRequestsPerWindow { get; set; } = 5;
    public int ResendWindowMinutes { get; set; } = 10;
}