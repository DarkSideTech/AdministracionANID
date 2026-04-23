using AUT2Services.Infra.Cross.IoC;
using AUT2Services.Infra.Security.Extensions;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.SecurityIoC;

namespace AUT2Services.Services.API.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Services == null) throw new ArgumentNullException(nameof(builder.Services));

        AI_DependencyInjectionConfig.Add_AI_DependencyInjectionConfiguration(builder.Services);
        NativeInjectorBootStrapper.RegisterServices(builder.Services);

        IdentityServiceExtensions.AddIdentityServices(builder);
        SecurityNativeInjectorBootStrapper.RegisterSecurityServices(builder.Services);
        PoliciesExtensions.AddPoliciesServices(builder.Services);

        return builder.Services;
    }
}

