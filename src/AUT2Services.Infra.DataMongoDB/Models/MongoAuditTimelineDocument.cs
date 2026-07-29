using AUT2Services.Domain.Core.Auditing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AUT2Services.Infra.DataMongoDB.Models;

public sealed class MongoAuditTimelineDocument
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; init; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid CorrelationId { get; init; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid AggregateId { get; init; }
    public string AggregateType { get; init; } = string.Empty;
    public long AggregateRevision { get; init; }
    public string EventType { get; init; } = string.Empty;
    public string CommandType { get; init; } = string.Empty;
    public AuditOperationType OperationType { get; init; }
    public DateTimeOffset OccurredAtUtc { get; init; }
    public DateTimeOffset PersistedAtUtc { get; init; }
    public string? RequestPath { get; init; }
    public AuditActor Actor { get; init; } = new(null, null, null);
    public string? SnapshotJson { get; init; }
    public IReadOnlyCollection<MongoAuditChangeDocument> Changes { get; init; } = [];
}
