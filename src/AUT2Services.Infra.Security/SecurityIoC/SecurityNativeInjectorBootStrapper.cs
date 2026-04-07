using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Accounts.Login;
using AUT2Services.Infra.Security.Accounts.LoginOrganizacion;
using AUT2Services.Infra.Security.Accounts.Logout;
using AUT2Services.Infra.Security.Accounts.RefreshToken;
using AUT2Services.Infra.Security.Accounts.Register;
using AUT2Services.Infra.Security.Accounts.ValidateEmail;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Infra.Tools.ZendeskManager;
using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.Security.SecurityIoC;

public class SecurityNativeInjectorBootStrapper
{
    public static void RegisterSecurityServices(IServiceCollection services)
    {
        services.AddScoped<ISecurityRepository, SecurityRepository>();
        services.AddScoped<IRequestHandler<BaseEntityCommand, CommandResponse>, BaseEntityCommandHandler>();
        services.AddScoped<IRequestHandler<LoginCommand, CommandResponse>, LoginCommandHandler>();
        services.AddScoped<IRequestHandler<LoginOrganizacionCommand, CommandResponse>, LoginOrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<LogoutCommand, CommandResponse>, LogoutCommandHandler>();
        services.AddScoped<IRequestHandler<RefreshTokenCommand, CommandResponse>, RefreshTokenCommandHandler>();
        services.AddScoped<IRequestHandler<RegisterCommand, CommandResponse>, RegisterCommandHandler>();
        services.AddScoped<IRequestHandler<EmailConfirmationTokenCommand, CommandResponse>, EmailConfirmationTokenCommandHandler>();

        services.AddScoped<IAccountServiceApp, AccountServiceApp>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<ITicketDataSender, ZendeskTicketSender>();

        services.AddScoped<IAuthCookieService, AuthCookieService>();
        services.AddScoped<ICsrfService, CsrfService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ISessionValidationService, SessionValidationService>();
        services.AddSingleton<IEmailConfirmationThrottleService, MemoryEmailConfirmationThrottleService>();
    }
}