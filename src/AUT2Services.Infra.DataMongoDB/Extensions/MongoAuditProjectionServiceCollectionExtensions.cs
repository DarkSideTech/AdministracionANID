using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.DataMongoDB.Configurations;
using AUT2Services.Infra.DataMongoDB.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.DataMongoDB.Extensions;

public static class MongoAuditProjectionServiceCollectionExtensions
{
    public static IServiceCollection AddMongoAuditProjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoAuditProjectionOptions>(
            configuration.GetSection(MongoAuditProjectionOptions.SectionName));
        services.AddSingleton<IAuditProjectionWriter, MongoAuditProjectionWriter>();
        services.AddHostedService<AuditOutboxMongoProjectionService>();

        return services;
    }
}
