namespace AUT2Services.Infra.DataMongoDB.Models;

public sealed class MongoAuditChangeDocument
{
    public int Order { get; init; }
    public string Path { get; init; } = string.Empty;
    public string? ValueType { get; init; }
    public string? NewValueJson { get; init; }
}
