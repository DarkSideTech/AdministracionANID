using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.Security.Extensions;

public class CorsExtensions
{
    public static void AddCorsConfiguration(IServiceCollection services, string[] allowedOrigins)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowMultiplesApp",
                builder => builder
                    .WithOrigins(allowedOrigins) // URL de las app
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });
    }
}
