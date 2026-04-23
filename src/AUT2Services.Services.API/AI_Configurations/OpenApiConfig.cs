// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-07
// -------------------------------------------------
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace AUT2Services.Services.API.Configurations;

public static class OpenApiConfig
{
    public static void AddOpenApiConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "v1.17",
                    Title = "Dominio AUT2Services",
                    Description = "Panel de Servicios Dominio AUT2Services API OpenAPI"
                };

                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                };

                var bearerRequirement = new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference("Bearer", document, "components/securitySchemes")
                    ] = []
                };

                if (document.Paths is null)
                {
                    return Task.CompletedTask;
                }

                foreach (var path in document.Paths.Values)
                {
                    if (path.Operations is null)
                    {
                        continue;
                    }

                    foreach (var operation in path.Operations.Values)
                    {
                        operation.Security ??= [];
                        operation.Security.Add(bearerRequirement);
                    }
                }

                return Task.CompletedTask;
            });
        });
    }

    public static void UseOpenApiSetup(this WebApplication app, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(configuration);

        if (!(app.Environment.IsDevelopment() || configuration.GetValue<bool>("ShowSwagger")))
        {
            return;
        }

        app.MapOpenApi("/openapi/{documentName}.json");

        if (configuration.GetValue<bool>("ShowSwagger"))
        {
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/openapi/v1.json", "v1.17");
                options.DocumentTitle = "AUT2Services OpenAPI";
            });
        }
    }
}
