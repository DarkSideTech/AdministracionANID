using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.DataMongoDB.Configurations;
using AUT2Services.Infra.DataMongoDB.Services;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Tests.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;

namespace AUT2Services.Tests.Integration.Auditing;

[TestClass]
[TestCategory("LocalDockerAudit")]
[DoNotParallelize]
public sealed class AuditOutboxMongoProjectionIntegrationTests
{
    [TestMethod]
    public async Task ProjectPendingAuditEntriesAsync_ProjectsMessageAndClearsLastError()
    {
        await using var fixture = await LocalAuditInfrastructureFixture.CreateAsync();
        var attemptedAtUtc = new DateTimeOffset(2026, 7, 29, 12, 0, 0, TimeSpan.Zero);
        var message = CreatePendingMessage(lastError: "previous local failure");

        await SeedAsync(fixture, message);

        using var serviceProvider = fixture.CreateServiceProvider(
            new MongoAuditProjectionWriter(Options.Create(fixture.CreateMongoProjectionOptions())));
        var service = CreateService(serviceProvider, fixture.CreateMongoProjectionOptions(), attemptedAtUtc);

        await ProjectPendingAuditEntriesAsync(service);

        var stored = await ReadMessageAsync(fixture, message.Id);
        Assert.AreEqual((short)1, stored.DispatchStatus);
        Assert.AreEqual(2, stored.DispatchAttempts);
        Assert.IsTrue(stored.LastDispatchAttemptUtc.HasValue);
        Assert.AreEqual(attemptedAtUtc, stored.LastDispatchAttemptUtc.GetValueOrDefault());
        Assert.IsTrue(stored.DispatchedAtUtc.HasValue);
        Assert.AreEqual(attemptedAtUtc, stored.DispatchedAtUtc.GetValueOrDefault());
        Assert.IsNull(stored.LastError);

        var idFilter = Builders<BsonDocument>.Filter.Eq(
            "_id",
            new BsonBinaryData(message.Id, GuidRepresentation.Standard));
        var projected = await fixture.MongoDatabase
            .GetCollection<BsonDocument>("audit_trail")
            .Find(idFilter)
            .SingleAsync();
        Assert.AreEqual(message.EventType, projected[nameof(AuditOutboxMessage.EventType)].AsString);
    }

    [TestMethod]
    public async Task ProjectPendingAuditEntriesAsync_RetriesAfterLocalMongoIsUnavailable()
    {
        await using var fixture = await LocalAuditInfrastructureFixture.CreateAsync();
        var firstAttemptAtUtc = new DateTimeOffset(2026, 7, 29, 13, 0, 0, TimeSpan.Zero);
        var retryAtUtc = firstAttemptAtUtc.AddMinutes(1);
        var message = CreatePendingMessage();

        await SeedAsync(fixture, message);

        using (var unavailableProvider = fixture.CreateServiceProvider(
                   new MongoAuditProjectionWriter(Options.Create(fixture.CreateUnavailableMongoProjectionOptions()))))
        {
            var unavailableService = CreateService(
                unavailableProvider,
                fixture.CreateUnavailableMongoProjectionOptions(),
                firstAttemptAtUtc);

            await ProjectPendingAuditEntriesAsync(unavailableService);
        }

        var afterFailure = await ReadMessageAsync(fixture, message.Id);
        Assert.AreEqual((short)0, afterFailure.DispatchStatus);
        Assert.AreEqual(1, afterFailure.DispatchAttempts);
        Assert.IsTrue(afterFailure.LastDispatchAttemptUtc.HasValue);
        Assert.AreEqual(firstAttemptAtUtc, afterFailure.LastDispatchAttemptUtc.GetValueOrDefault());
        Assert.IsFalse(afterFailure.DispatchedAtUtc.HasValue);
        Assert.IsFalse(string.IsNullOrWhiteSpace(afterFailure.LastError));

        using (var availableProvider = fixture.CreateServiceProvider(
                   new MongoAuditProjectionWriter(Options.Create(fixture.CreateMongoProjectionOptions()))))
        {
            var availableService = CreateService(
                availableProvider,
                fixture.CreateMongoProjectionOptions(),
                retryAtUtc);

            await ProjectPendingAuditEntriesAsync(availableService);
        }

        var afterRetry = await ReadMessageAsync(fixture, message.Id);
        Assert.AreEqual((short)1, afterRetry.DispatchStatus);
        Assert.AreEqual(2, afterRetry.DispatchAttempts);
        Assert.IsTrue(afterRetry.LastDispatchAttemptUtc.HasValue);
        Assert.AreEqual(retryAtUtc, afterRetry.LastDispatchAttemptUtc.GetValueOrDefault());
        Assert.IsTrue(afterRetry.DispatchedAtUtc.HasValue);
        Assert.AreEqual(retryAtUtc, afterRetry.DispatchedAtUtc.GetValueOrDefault());
        Assert.IsNull(afterRetry.LastError);
    }

    private static AuditOutboxMongoProjectionService CreateService(
        ServiceProvider serviceProvider,
        MongoAuditProjectionOptions options,
        DateTimeOffset attemptedAtUtc)
        => new(
            serviceProvider.GetRequiredService<IServiceScopeFactory>(),
            Options.Create(options),
            new TestClock(attemptedAtUtc),
            NullLogger<AuditOutboxMongoProjectionService>.Instance);

    private static async Task ProjectPendingAuditEntriesAsync(AuditOutboxMongoProjectionService service)
    {
        var method = typeof(AuditOutboxMongoProjectionService).GetMethod(
            "ProjectPendingAuditEntriesAsync",
            BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new AssertFailedException("Audit outbox projection cycle was not found.");
        var operation = method.Invoke(service, [CancellationToken.None]) as Task
            ?? throw new AssertFailedException("Audit outbox projection cycle did not return a task.");

        await operation;
    }

    private static async Task SeedAsync(LocalAuditInfrastructureFixture fixture, AuditOutboxMessage message)
    {
        await using var dbContext = fixture.CreateDbContext();
        dbContext.AuditOutboxMessages.Add(message);
        await dbContext.SaveChangesAsync();
    }

    private static async Task<AuditOutboxMessage> ReadMessageAsync(LocalAuditInfrastructureFixture fixture, Guid id)
    {
        await using var dbContext = fixture.CreateDbContext();
        return await dbContext.AuditOutboxMessages
            .AsNoTracking()
            .SingleAsync(message => message.Id == id);
    }

    private static AuditOutboxMessage CreatePendingMessage(string? lastError = null)
    {
        var occurredAtUtc = new DateTimeOffset(2026, 7, 29, 11, 59, 0, TimeSpan.Zero);
        var message = new AuditOutboxMessage
        {
            Id = Guid.NewGuid(),
            CorrelationId = Guid.NewGuid(),
            AggregateId = Guid.NewGuid(),
            AggregateType = "Usuario",
            AggregateRevision = 1,
            EventType = "Audit.User.Updated",
            CommandType = "ActualizarUsuarioCommand",
            OperationType = AuditOperationType.Update,
            ActorUserId = "user-1",
            ActorUsername = "local-tester",
            ActorEmail = "local-tester@example.test",
            RequestPath = "/usuarios/123",
            OccurredAtUtc = occurredAtUtc,
            PersistedAtUtc = occurredAtUtc,
            SnapshotJson = "{\"status\":\"updated\"}",
            DispatchStatus = 0,
            DispatchAttempts = lastError is null ? 0 : 1,
            LastError = lastError,
            SchemaVersion = 1
        };

        message.Changes.Add(new AuditOutboxChange
        {
            Id = Guid.NewGuid(),
            Order = 0,
            Path = "Status",
            ValueType = "String",
            NewValueJson = "\"updated\""
        });

        return message;
    }
}
