using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataMongoDB.Configurations;
using AUT2Services.Infra.Migrations.PostgreSql.Migrations;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Npgsql;
using System.Net;

namespace AUT2Services.Tests.Integration.Auditing;

internal sealed class LocalAuditEnvironment
{
    internal const string DatabaseName = "aut2_local_test";
    internal const string PostgresConnectionVariable = "AUT2_LOCAL_TEST_POSTGRES_CONNECTION";
    internal const string MongoConnectionVariable = "AUT2_LOCAL_TEST_MONGO_CONNECTION";

    internal void ConfigurePostgres(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            GetLocalPostgresConnectionString(),
            npgsql => npgsql.MigrationsAssembly(typeof(AddAuditTraceability).Assembly.FullName));
    }

    internal AUT2ServicesContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AUT2ServicesContext>();
        ConfigurePostgres(optionsBuilder);
        return new AUT2ServicesContext(optionsBuilder.Options);
    }

    internal MongoClient CreateMongoClient()
    {
        var settings = MongoClientSettings.FromConnectionString(GetLocalMongoConnectionString());
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        return new MongoClient(settings);
    }

    internal MongoAuditProjectionOptions CreateMongoProjectionOptions()
        => new()
        {
            Enabled = true,
            ConnectionString = GetLocalMongoConnectionString(),
            DatabaseName = DatabaseName,
            CollectionName = "audit_trail",
            BatchSize = 10,
            IntervalSeconds = 60
        };

    internal MongoAuditProjectionOptions CreateUnavailableMongoProjectionOptions()
    {
        var connectionString = GetLocalMongoConnectionString();
        var sourceUrl = new MongoUrl(connectionString);
        var localServer = sourceUrl.Servers.First();
        var localHost = localServer.Host;
        if (string.IsNullOrWhiteSpace(localHost))
        {
            throw new AssertInconclusiveException(
                $"{MongoConnectionVariable} must include a local MongoDB host.");
        }

        var unavailableUrl = new MongoUrlBuilder(connectionString)
        {
            Server = new MongoServerAddress(localHost, 1),
            ServerSelectionTimeout = TimeSpan.FromMilliseconds(250),
            ConnectTimeout = TimeSpan.FromMilliseconds(250),
            SocketTimeout = TimeSpan.FromMilliseconds(250)
        };

        return new MongoAuditProjectionOptions
        {
            Enabled = true,
            ConnectionString = unavailableUrl.ToMongoUrl().ToString(),
            DatabaseName = DatabaseName,
            CollectionName = "audit_trail",
            BatchSize = 10,
            IntervalSeconds = 60
        };
    }

    private static string GetLocalPostgresConnectionString()
    {
        var connectionString = GetRequiredEnvironmentVariable(PostgresConnectionVariable);
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var postgresHost = builder.Host;

        if (string.IsNullOrWhiteSpace(postgresHost))
        {
            throw new AssertInconclusiveException(
                $"{PostgresConnectionVariable} must include a PostgreSQL loopback host.");
        }

        EnsureLoopbackHost(postgresHost, PostgresConnectionVariable);
        builder.Database = DatabaseName;

        return builder.ConnectionString;
    }

    private static string GetLocalMongoConnectionString()
    {
        var connectionString = GetRequiredEnvironmentVariable(MongoConnectionVariable);
        var url = new MongoUrl(connectionString);

        if (!connectionString.StartsWith("mongodb://", StringComparison.OrdinalIgnoreCase)
            || url.Servers.Count() != 1
            || url.Servers.Any(server => !IsLoopbackHost(server.Host)))
        {
            throw new AssertInconclusiveException(
                $"{MongoConnectionVariable} must target one local MongoDB endpoint using the mongodb scheme.");
        }

        return new MongoUrlBuilder(connectionString)
        {
            DirectConnection = true
        }.ToMongoUrl().ToString();
    }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new AssertInconclusiveException(
                $"Set the local-only environment variable {variableName} before running LocalDockerAudit tests.");
        }

        return value;
    }

    private static void EnsureLoopbackHost(string host, string variableName)
    {
        if (!IsLoopbackHost(host))
        {
            throw new AssertInconclusiveException(
                $"{variableName} must target a PostgreSQL loopback endpoint.");
        }
    }

    private static bool IsLoopbackHost(string host)
        => string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
           || (IPAddress.TryParse(host, out var address) && IPAddress.IsLoopback(address));
}
