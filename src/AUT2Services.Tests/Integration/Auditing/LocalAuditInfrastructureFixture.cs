using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataMongoDB.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AUT2Services.Tests.Integration.Auditing;

internal sealed class LocalAuditInfrastructureFixture : IAsyncDisposable
{
    private const string MarkerCollectionName = "_aut2_local_audit_marker";
    private const string MarkerId = "fixture-ready";
    private static readonly SemaphoreSlim Gate = new(1, 1);

    private readonly LocalAuditEnvironment environment = new();
    private readonly MongoClient mongoClient;
    private int disposed;

    private LocalAuditInfrastructureFixture()
    {
        mongoClient = environment.CreateMongoClient();
    }

    internal IMongoDatabase MongoDatabase => mongoClient.GetDatabase(LocalAuditEnvironment.DatabaseName);

    internal AUT2ServicesContext CreateDbContext() => environment.CreateDbContext();

    internal ServiceProvider CreateServiceProvider(IAuditProjectionWriter projectionWriter)
    {
        var services = new ServiceCollection();
        services.AddDbContext<AUT2ServicesContext>(environment.ConfigurePostgres);
        services.AddSingleton<IAuditProjectionWriter>(projectionWriter);

        return services.BuildServiceProvider(validateScopes: true);
    }

    internal MongoAuditProjectionOptions CreateMongoProjectionOptions()
        => environment.CreateMongoProjectionOptions();

    internal MongoAuditProjectionOptions CreateUnavailableMongoProjectionOptions()
        => environment.CreateUnavailableMongoProjectionOptions();

    internal static async Task<LocalAuditInfrastructureFixture> CreateAsync(CancellationToken cancellationToken = default)
    {
        await Gate.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var fixture = new LocalAuditInfrastructureFixture();
            await fixture.ResetAsync(cancellationToken).ConfigureAwait(false);
            return fixture;
        }
        catch
        {
            Gate.Release();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref disposed, 1) != 0)
        {
            return;
        }

        try
        {
            await ResetAsync(CancellationToken.None).ConfigureAwait(false);
        }
        finally
        {
            Gate.Release();
        }
    }

    private async Task ResetAsync(CancellationToken cancellationToken)
    {
        await ResetPostgresAsync(cancellationToken).ConfigureAwait(false);
        await ResetMongoAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task ResetPostgresAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = CreateDbContext();
        await dbContext.Database.EnsureDeletedAsync(cancellationToken).ConfigureAwait(false);
        await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task ResetMongoAsync(CancellationToken cancellationToken)
    {
        var collectionNames = await (await MongoDatabase
                .ListCollectionNamesAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        foreach (var collectionName in collectionNames.Where(name => !string.Equals(
                     name,
                     MarkerCollectionName,
                     StringComparison.Ordinal)))
        {
            await MongoDatabase.DropCollectionAsync(collectionName, cancellationToken).ConfigureAwait(false);
        }

        var marker = new BsonDocument
        {
            ["_id"] = MarkerId,
            ["purpose"] = "aut2-local-audit-fixture"
        };

        var markerCollection = MongoDatabase.GetCollection<BsonDocument>(MarkerCollectionName);
        await markerCollection.ReplaceOneAsync(
                Builders<BsonDocument>.Filter.Eq("_id", MarkerId),
                marker,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken)
            .ConfigureAwait(false);
    }
}
