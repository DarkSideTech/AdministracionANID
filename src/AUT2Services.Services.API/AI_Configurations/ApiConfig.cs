// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.939
// -------------------------------------------------
namespace AUT2Services.Services.API.Configurations;

public static class ApiConfig
{
    public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Configuration
                    .SetBasePath(builder.Environment.ContentRootPath)
                    .AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                    .AddEnvironmentVariables();

        builder.Services.AddControllers();

        return builder;
    }
}

