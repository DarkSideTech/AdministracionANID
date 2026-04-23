using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.DataMongoDB.Configurations;
using AUT2Services.Infra.DataMongoDB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AUT2Services.Infra.DataMongoDB.Services;

public sealed class MongoAuditProjectionWriter : IAuditProjectionWriter
{
    private readonly IMongoCollection<MongoAuditTimelineDocument>? collection;

    public MongoAuditProjectionWriter(IOptions<MongoAuditProjectionOptions> options)
    {
        var currentOptions = options.Value;
        if (!currentOptions.Enabled
            || string.IsNullOrWhiteSpace(currentOptions.ConnectionString)
            || string.IsNullOrWhiteSpace(currentOptions.DatabaseName))
        {
            return;
        }

        var client = new MongoClient(currentOptions.ConnectionString);
        var database = client.GetDatabase(currentOptions.DatabaseName);
        collection = database.GetCollection<MongoAuditTimelineDocument>(currentOptions.CollectionName);
    }

    public async Task WriteAsync(IReadOnlyCollection<AuditEnvelope> envelopes, CancellationToken cancellationToken = default)
    {
        if (collection is null || envelopes.Count == 0)
        {
            return;
        }

        var models = envelopes
            .Select(envelope =>
            {
                var document = new MongoAuditTimelineDocument
                {
                    Id = envelope.Id,
                    CorrelationId = envelope.CorrelationId,
                    AggregateId = envelope.AggregateId,
                    AggregateType = envelope.AggregateType,
                    AggregateRevision = envelope.AggregateRevision,
                    EventType = envelope.EventType,
                    CommandType = envelope.CommandType,
                    OperationType = envelope.OperationType,
                    OccurredAtUtc = envelope.OccurredAtUtc,
                    PersistedAtUtc = envelope.PersistedAtUtc,
                    RequestPath = envelope.RequestPath,
                    Actor = envelope.Actor,
                    SnapshotJson = envelope.SnapshotJson,
                    Changes = envelope.Changes
                        .OrderBy(change => change.Order)
                        .Select(change => new MongoAuditChangeDocument
                        {
                            Order = change.Order,
                            Path = change.Path,
                            ValueType = change.ValueType,
                            NewValueJson = change.NewValueJson
                        })
                        .ToArray()
                };

                return new ReplaceOneModel<MongoAuditTimelineDocument>(
                    Builders<MongoAuditTimelineDocument>.Filter.Eq(item => item.Id, document.Id),
                    document)
                {
                    IsUpsert = true
                };
            })
            .ToArray();

        if (models.Length == 0)
        {
            return;
        }

        await collection.BulkWriteAsync(models, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
