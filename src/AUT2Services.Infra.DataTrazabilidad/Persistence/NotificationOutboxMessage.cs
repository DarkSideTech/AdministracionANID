namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class NotificationOutboxMessage
{
    public Guid Id { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string NotificationType { get; set; } = string.Empty;
    public string? UserId { get; set; }
    public string? RequestPath { get; set; }
    public string? DeduplicationKey { get; set; }
    public string PayloadJson { get; set; } = string.Empty;
    public short DispatchStatus { get; set; }
    public int DispatchAttempts { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset NextAttemptUtc { get; set; }
    public DateTimeOffset? LastDispatchAttemptUtc { get; set; }
    public DateTimeOffset? DispatchedAtUtc { get; set; }
    public string? LastError { get; set; }
    public short SchemaVersion { get; set; }
}
