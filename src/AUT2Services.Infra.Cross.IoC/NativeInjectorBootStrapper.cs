using AUT2Services.Application.Interfaces;
using AUT2Services.Application.Services.ServicioDeDominioHandlers;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Repositories;
using AUT2Services.Infra.Tools.EmailManager;
using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.Cross.IoC;

public class NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IServicioDeDominioServiceApp, ServicioDeDominioServiceApp>();
        services.AddScoped<IServicioDeDominioRepository, ServicioDeDominioRepository>();
        services.AddScoped<IEmailMessageSender, MailkitEmailSender>();
    }
}

