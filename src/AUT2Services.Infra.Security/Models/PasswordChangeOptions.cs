namespace AUT2Services.Infra.Security.Models;

public sealed class PasswordChangeOptions
{
    public const string PasswordChangeOptionsKey = "PasswordChange";

    public int CodeLength { get; set; } = 6;
    public int CodeLifetimeMinutes { get; set; } = 10;
    public int ResendCooldownSeconds { get; set; } = 60;
    public int MaxFailedAttempts { get; set; } = 5;
}
