using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.DataMongoDB.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AUT2Services.Tests.Integration.Auditing;

[TestClass]
[TestCategory("LocalDockerAudit")]
[DoNotParallelize]
public sealed class MongoAuditProjectionWriterIntegrationTests
{
    [TestMethod]
    public async Task MongoLocal_RespondsToPing()
    {
        await using var fixture = await LocalAuditInfrastructureFixture.CreateAsync();

        var response = await fixture.MongoDatabase.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));

        Assert.AreEqual(1d, response["ok"].ToDouble());
    }

    [TestMethod]
    public async Task WriteAsync_UsesStandardBsonUuidsAndUpsertsById()
    {
        await using var fixture = await LocalAuditInfrastructureFixture.CreateAsync();
        var writer = new MongoAuditProjectionWriter(Options.Create(fixture.CreateMongoProjectionOptions()));
        var first = CreateEnvelope(Guid.NewGuid(), "Audit.User.Updated");

        await writer.WriteAsync([first]);

        var collection = fixture.MongoDatabase.GetCollection<BsonDocument>("audit_trail");
        var idFilter = Builders<BsonDocument>.Filter.Eq(
            "_id",
            new BsonBinaryData(first.Id, GuidRepresentation.Standard));
        var stored = await collection.Find(idFilter).SingleAsync();

        AssertStandardGuid(stored, "_id", first.Id);
        AssertStandardGuid(stored, nameof(AuditEnvelope.CorrelationId), first.CorrelationId);
        AssertStandardGuid(stored, nameof(AuditEnvelope.AggregateId), first.AggregateId);

        var replacement = CreateEnvelope(first.Id, "Audit.User.Reprojected", first.CorrelationId, first.AggregateId);
        await writer.WriteAsync([replacement]);

        Assert.AreEqual(1L, await collection.CountDocumentsAsync(idFilter));
        var upserted = await collection.Find(idFilter).SingleAsync();
        Assert.AreEqual("Audit.User.Reprojected", upserted[nameof(AuditEnvelope.EventType)].AsString);
    }

    private static AuditEnvelope CreateEnvelope(
        Guid id,
        string eventType,
        Guid? correlationId = null,
        Guid? aggregateId = null)
    {
        var now = DateTimeOffset.UtcNow;
        return new AuditEnvelope
        {
            Id = id,
            CorrelationId = correlationId ?? Guid.NewGuid(),
            AggregateId = aggregateId ?? Guid.NewGuid(),
            AggregateType = "Usuario",
            AggregateRevision = 1,
            EventType = eventType,
            CommandType = "ActualizarUsuarioCommand",
            OperationType = AuditOperationType.Update,
            OccurredAtUtc = now,
            PersistedAtUtc = now,
            RequestPath = "/usuarios/123",
            Actor = new AuditActor("user-1", "local-tester", "local-tester@example.test"),
            SnapshotJson = "{\"status\":\"updated\"}",
            Changes = [new AuditDeltaChange(0, "Status", "String", "\"updated\"")]
        };
    }

    private static void AssertStandardGuid(BsonDocument document, string fieldName, Guid expected)
    {
        Assert.IsTrue(document.Contains(fieldName), $"Expected BSON field {fieldName}.");

        var value = document[fieldName].AsBsonBinaryData;
        Assert.AreEqual(BsonBinarySubType.UuidStandard, value.SubType);
        Assert.AreEqual(expected, value.ToGuid());
    }
}
