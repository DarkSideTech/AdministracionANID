// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.449
// -------------------------------------------------
namespace AUT2Services.Services.API.Configurations;

public static class ApiConfig
{
    public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.Configuration
                    .SetBasePath(builder.Environment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables();

        builder.Services.AddControllers();

        return builder;
    }
}

