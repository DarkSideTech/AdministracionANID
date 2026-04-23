namespace AUT2Services.Infra.DataMongoDB.Configurations;

public sealed class MongoAuditProjectionOptions
{
    public const string SectionName = "MongoAuditProjection";

    public bool Enabled { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string CollectionName { get; set; } = "audit_trail";
    public int BatchSize { get; set; } = 100;
    public int IntervalSeconds { get; set; } = 5;
}
