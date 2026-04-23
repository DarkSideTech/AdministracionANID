namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class AuditAggregateCursor
{
    public Guid AggregateId { get; set; }
    public long LastRevision { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public Guid ConcurrencyToken { get; set; }
}
