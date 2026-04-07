// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 11:34:56.414
// -------------------------------------------------
using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Services.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiConfiguration();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

builder.Services.Configure<SendEmailOptions>(
    builder.Configuration.GetSection(SendEmailOptions.EmailOptionsKey));

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.AddDependencyInjectionConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.AddCorsConfiguration(builder.Environment.IsDevelopment());

//builder.Services.AddEndpointsApiExplorer(); // Essential for Minimal APIs with Swagger
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

if (builder.Configuration["DB_PROVIDER"] == "postgresql")
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
}

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
    app.UseSwaggerSetup(builder.Configuration);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
	throw;
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowDinamicRules");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();