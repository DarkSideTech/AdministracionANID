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
                var hasWildcardOrigin = allowedOrigins.Any(origin => origin.Trim() == "*");

                if (environment_develop || hasWildcardOrigin)
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
