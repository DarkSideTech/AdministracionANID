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
                    var configuredOrigins = allowedOrigins
                        .Where(origin => !string.IsNullOrWhiteSpace(origin))
                        .Select(origin => origin.Trim().TrimEnd('/'))
                        .Where(origin => !origin.Equals("*", StringComparison.Ordinal))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();

                    if (configuredOrigins.Length == 0)
                    {
                        policy.SetIsOriginAllowed(_ => false);
                        return;
                    }

                    policy
                        .WithOrigins(configuredOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            });
        });
    }
}
