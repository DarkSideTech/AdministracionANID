// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.449
// -------------------------------------------------
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Services.API.Configurations;

public static class DatabaseConfig
{
    public const string PostgreSql = "postgresql";
    public const string SqlServer = "sqlserver";
    public const string DatabaseProviderKey = "DB_PROVIDER";
    public const string DefaultConnectionStringKey = "AUT2ServicesConnection";
    public const string PostgreSqlConnectionStringKey = "AUT2ServicesConnectionPostgreSql";
    public const string SqlServerConnectionStringKey = "AUT2ServicesConnectionSqlServer";

    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var databaseProvider = ResolveDatabaseProvider(configuration);
        var connectionString = ResolveConnectionString(configuration, databaseProvider);

        services.AddDbContext<AUT2ServicesContext>(
            options => _ = databaseProvider switch
            {
                PostgreSql =>
                  options.UseNpgsql(
                    connectionString,
                    x => x.MigrationsAssembly("AUT2Services.Infra.Migrations.PostgreSql")),

                SqlServer => options.UseSqlServer(
                    connectionString,
                    x => x.MigrationsAssembly("AUT2Services.Infra.Migrations.SqlServer")),

                _ => throw new NotSupportedException($"Database provider without configuration: {databaseProvider}")
            })
            .AddOptions();
    }

    public static string ResolveDatabaseProvider(IConfiguration configuration)
    {
        var databaseProvider = configuration[DatabaseProviderKey];

        if (string.IsNullOrWhiteSpace(databaseProvider))
        {
            throw new NotSupportedException($"Database provider es requerido en {DatabaseProviderKey}.");
        }

        databaseProvider = databaseProvider.Trim().ToLowerInvariant();

        return databaseProvider switch
        {
            PostgreSql or SqlServer => databaseProvider,
            _ => throw new NotSupportedException($"Database provider without configuration: {databaseProvider}")
        };
    }

    private static string ResolveConnectionString(IConfiguration configuration, string databaseProvider)
    {
        var explicitConnectionString = configuration.GetConnectionString(DefaultConnectionStringKey);
        if (!string.IsNullOrWhiteSpace(explicitConnectionString))
        {
            return explicitConnectionString;
        }

        var providerConnectionStringKey = databaseProvider switch
        {
            PostgreSql => PostgreSqlConnectionStringKey,
            SqlServer => SqlServerConnectionStringKey,
            _ => throw new NotSupportedException($"Database provider without configuration: {databaseProvider}")
        };

        var providerConnectionString = configuration.GetConnectionString(providerConnectionStringKey);
        if (!string.IsNullOrWhiteSpace(providerConnectionString))
        {
            return providerConnectionString;
        }

        throw new InvalidOperationException($"No existe connection string para el provider [{databaseProvider}].");
    }
}

