using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.Security.Extensions;

public class CorsExtensions
{
    public static void AddCorsConfiguration(IServiceCollection services, string[] allowedOrigins, bool environment_develop)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowDinamicRules", policy =>
            {
                if (environment_develop)
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                }
                else
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            });
        });
    }
}
