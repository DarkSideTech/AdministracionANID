namespace AUT2Services.Infra.Security.Models;

public sealed class NotificationOutboxOptions
{
    public const string NotificationOutboxOptionsKey = "NotificationOutbox";

    public bool Enabled { get; set; } = true;
    public int BatchSize { get; set; } = 50;
    public int IntervalSeconds { get; set; } = 5;
    public int MaxAttempts { get; set; } = 5;
    public int InitialRetryDelaySeconds { get; set; } = 30;
    public int MaxRetryDelaySeconds { get; set; } = 900;
}
