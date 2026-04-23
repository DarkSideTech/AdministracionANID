namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class PasswordChangeChallenge
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CodeHash { get; set; } = string.Empty;
    public string? RequestPath { get; set; }
    public int FailedAttempts { get; set; }
    public int ResendCount { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset LastSentAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }
    public DateTimeOffset? ConsumedAtUtc { get; set; }
    public DateTimeOffset? CancelledAtUtc { get; set; }
}
