// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 11:34:56.414
// -------------------------------------------------
using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.DataMongoDB.Extensions;
using AUT2Services.Services.API.Configurations;
using AUT2Services.Infra.Security.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

builder.Services.AddExternalIntegrationOptions(builder.Configuration);

builder.Services.Configure<ClaveUnicaOptions>(
    builder.Configuration.GetSection(ClaveUnicaOptions.ClaveUnicaOptionsKey));

builder.Services.AddMemoryCache();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddMongoAuditProjection(builder.Configuration);
builder.AddDependencyInjectionConfiguration();
builder.Services.AddOpenApiConfiguration();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.AddCorsConfiguration(builder.Environment.IsDevelopment());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

try
{
    app.UseOpenApiSetup(builder.Configuration);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
	throw;
}

app.UseCors("AllowDinamicRules");
app.UseRateLimiter();
app.UseAuthentication();
app.UseMiddleware<AuditExecutionContextMiddleware>();
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "AUT2Services.Services.API"
}));
app.MapHealthChecks("/healthz");
app.MapControllers();

app.Run();
