using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;
using AUT2Services.Infra.Security.Accounts.CurrentUser;
using AUT2Services.Infra.Security.Accounts.EmailConfirmationToken;
using AUT2Services.Infra.Security.Accounts.Login;
using AUT2Services.Infra.Security.Accounts.LoginClaveUnica;
using AUT2Services.Infra.Security.Accounts.LoginOrganizacion;
using AUT2Services.Infra.Security.Accounts.Logout;
using AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;
using AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.AdminModificaCorreoElectronico;
using AUT2Services.Infra.Security.Accounts.ActivarUsuario;
using AUT2Services.Infra.Security.Accounts.BuscarUsuariosPaginados;
using AUT2Services.Infra.Security.Accounts.DesactivarUsuario;
using AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;
using AUT2Services.Infra.Security.Accounts.ModificaUsuario;
using AUT2Services.Infra.Security.Accounts.RefreshToken;
using AUT2Services.Infra.Security.Accounts.Register;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.ResendEmailConfirmationToken;
using AUT2Services.Infra.Security.Accounts.Roles;
using AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;
using AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.Yo;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Services;
using AUT2Services.Infra.Tools.ZendeskManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.SecurityIoC;

public class SecurityNativeInjectorBootStrapper
{
    public static void RegisterSecurityServices(IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<BaseEntityCommand, CommandResponse>, BaseEntityCommandHandler>();
        services.AddScoped<IRequestHandler<CambioUnidadOrganizacionalEntidadRolCommand, CommandResponse>, CambioUnidadOrganizacionalEntidadRolCommandHandler>();
        services.AddScoped<IRequestHandler<CurrentUserCommand, CommandResponse>, CurrentUserCommandHandler>();
        services.AddScoped<IRequestHandler<EmailConfirmationTokenCommand, CommandResponse>, EmailConfirmationTokenCommandHandler>();
        services.AddScoped<IRequestHandler<LoginCommand, CommandResponse>, LoginCommandHandler>();
        services.AddScoped<IRequestHandler<LoginClaveUnicaCommand, CommandResponse>, LoginClaveUnicaCommandHandler>();
        services.AddScoped<IRequestHandler<LoginOrganizacionCommand, CommandResponse>, LoginOrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<LogoutCommand, CommandResponse>, LogoutCommandHandler>();
        services.AddScoped<IRequestHandler<SolicitaCambioClaveCommand, CommandResponse>, SolicitaCambioClaveCommandHandler>();
        services.AddScoped<IRequestHandler<ReenviaCodigoCambioClaveCommand, CommandResponse>, ReenviaCodigoCambioClaveCommandHandler>();
        services.AddScoped<IRequestHandler<ConfirmaCambioClaveCommand, CommandResponse>, ConfirmaCambioClaveCommandHandler>();
        services.AddScoped<IRequestHandler<SolicitaRecuperacionClaveCommand, CommandResponse>, SolicitaRecuperacionClaveCommandHandler>();
        services.AddScoped<IRequestHandler<ReenviaCodigoRecuperacionClaveCommand, CommandResponse>, ReenviaCodigoRecuperacionClaveCommandHandler>();
        services.AddScoped<IRequestHandler<ConfirmaRecuperacionClaveCommand, CommandResponse>, ConfirmaRecuperacionClaveCommandHandler>();
        services.AddScoped<IRequestHandler<AdminModificaCorreoElectronicoCommand, CommandResponse>, AdminModificaCorreoElectronicoCommandHandler>();
        services.AddScoped<IRequestHandler<ModificaCorreoElectronicoCommand, CommandResponse>, ModificaCorreoElectronicoCommandHandler>();
        services.AddScoped<IRequestHandler<RefreshTokenCommand, CommandResponse>, RefreshTokenCommandHandler>();
        services.AddScoped<IRequestHandler<RegisterCommand, CommandResponse>, RegisterCommandHandler>();
        services.AddScoped<IRequestHandler<ResendEmailConfirmationTokenCommand, CommandResponse>, ResendEmailConfirmationTokenCommandHandler>();
        services.AddScoped<IRequestHandler<YoCommand, CommandResponse>, YoCommandHandler>();
        services.AddScoped<IRequestHandler<ModificaUsuarioCommand, CommandResponse>, ModificaUsuarioCommandHandler>();
        services.AddScoped<IRequestHandler<BuscarUsuariosPaginadosCommand, CommandResponse>, BuscarUsuariosPaginadosCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarUsuarioCommand, CommandResponse>, ActivarUsuarioCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarUsuarioCommand, CommandResponse>, DesactivarUsuarioCommandHandler>();
        services.AddScoped<IRequestHandler<BuscarRolesPaginadosCommand, CommandResponse>, BuscarRolesPaginadosCommandHandler>();
        services.AddScoped<IRequestHandler<ModificaRolCommand, CommandResponse>, ModificaRolCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarRolCommand, CommandResponse>, ActivarRolCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarRolCommand, CommandResponse>, DesactivarRolCommandHandler>();
        services.AddScoped<IRequestHandler<RequiereValidacionAlSerAsignadoCommand, CommandResponse>, RequiereValidacionAlSerAsignadoCommandHandler>();
        services.AddScoped<IRequestHandler<NoRequiereValidacionAlSerAsignadoCommand, CommandResponse>, NoRequiereValidacionAlSerAsignadoCommandHandler>();
        services.AddScoped<IRequestHandler<ActivaValidacionDeAsignacionDeRolesCommand, CommandResponse>, ActivaValidacionDeAsignacionDeRolesCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivaValidacionDeAsignacionDeRolesCommand, CommandResponse>, DesactivaValidacionDeAsignacionDeRolesCommandHandler>();
        services.AddScoped<IRequestHandler<ActivaDetalleDeAutorizacionesCommand, CommandResponse>, ActivaDetalleDeAutorizacionesCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivaDetalleDeAutorizacionesCommand, CommandResponse>, DesactivaDetalleDeAutorizacionesCommandHandler>();

        services.AddScoped<IAccountServiceApp, AccountServiceApp>();
        services.AddScoped<IAuthCookieService, AuthCookieService>();
        services.AddScoped<IClaveUnicaClient, ClaveUnicaClient>();
        services.AddScoped<ICsrfService, CsrfService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IEmailConfirmationMessageService, EmailConfirmationMessageService>();
        services.AddScoped<INotificationOutboxService, NotificationOutboxService>();
        services.AddScoped<IPasswordChangeChallengeMessageService, PasswordChangeChallengeMessageService>();
        services.AddScoped<ISecurityTraceabilityService, SecurityTraceabilityService>();
        services.AddScoped<INotificationChannelDispatcher, EmailNotificationChannelDispatcher>();
        services.AddScoped<INotificationChannelDispatcher, ZendeskNotificationChannelDispatcher>();
        services.AddHttpClient();
        services.AddHttpClient(ClaveUnicaClient.HttpClientName, client =>
        {
            client.Timeout = TimeSpan.FromSeconds(15);
        });
        services.AddHttpClient<ITicketDataSender, ZendeskTicketSender>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ZendeskSendTicketOptions>>().Value;
            client.BaseAddress = new Uri(options.Subdomain.TrimEnd('/'));
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("AUT2Services/ZendeskTicketSender");
        });
        services.AddSingleton<IEmailConfirmationThrottleService, MemoryEmailConfirmationThrottleService>();
        services.AddScoped<ISecurityRepository, SecurityRepository>();
        services.AddScoped<ISessionValidationService, SessionValidationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddHostedService<NotificationOutboxDispatcherService>();
    }
}
