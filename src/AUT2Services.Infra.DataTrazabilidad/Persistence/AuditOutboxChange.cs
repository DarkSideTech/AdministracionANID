namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class AuditOutboxChange
{
    public Guid Id { get; set; }
    public Guid AuditOutboxMessageId { get; set; }
    public int Order { get; set; }
    public string Path { get; set; } = string.Empty;
    public string? ValueType { get; set; }
    public string? NewValueJson { get; set; }

    public AuditOutboxMessage AuditOutboxMessage { get; set; } = default!;
}
