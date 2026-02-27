// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.335
// -------------------------------------------------
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Services.API.Configurations;

public static class DatabaseConfig
{
    public const string PostgreSql = "postgresql";
    public const string SqlServer = "sqlserver";

    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var database_provider = configuration["ASPNETCORE_DB_PROVIDER"];

        if (string.IsNullOrEmpty(database_provider))
        {
            database_provider = configuration["DB_PROVIDER"];
        }

        if (string.IsNullOrEmpty(database_provider))
        {
            throw new NotSupportedException("Database provider es requerido.");
        }

        string connectionString = configuration.GetConnectionString("AUT2ServicesConnection")!;

        services.AddDbContext<AUT2ServicesContext>(
            options => _ = database_provider switch
            {
                PostgreSql =>
                  options
                    .UseNpgsql(
                    connectionString,
                    x => x.MigrationsAssembly("AUT2Services.Infra.Migrations.PostgreSql")),                    

                SqlServer => options.UseSqlServer(
                    connectionString,
                    x => x.MigrationsAssembly("AUT2Services.Infra.Migrations.SqlServer")),

                _ => throw new NotSupportedException($"Database provider without configuration: {database_provider}")
            })
            .AddOptions();
    }
}

