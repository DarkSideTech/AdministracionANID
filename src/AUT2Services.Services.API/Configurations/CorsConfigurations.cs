using AUT2Services.Infra.Security.Extensions;

namespace AUT2Services.Services.API.Configurations;

public static class CorsConfigurations
{
    public static IServiceCollection AddCorsConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Services == null) throw new ArgumentNullException(nameof(builder.Services));

        var allowedCorsOrigins = builder.Configuration.GetSection("JwtOptions:AllowedCorsOrigins").Get<string[]>();

        CorsExtensions.AddCorsConfiguration(builder.Services, allowedCorsOrigins!);

        return builder.Services;
    }
}
