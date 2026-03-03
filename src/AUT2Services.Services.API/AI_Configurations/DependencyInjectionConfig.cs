// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.939
// -------------------------------------------------
using AUT2Services.Infra.Cross.IoC;

namespace AUT2Services.Services.API.Configurations;

public static class AI_DependencyInjectionConfig
{
    public static IServiceCollection Add_AI_DependencyInjectionConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        AI_NativeInjectorBootStrapper.RegisterServices(services);

        return services;
    }
}

