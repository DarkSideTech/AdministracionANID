using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace AUT2Services.Services.API.Configurations;

public static class ExternalIntegrationOptionsConfig
{
    public static IServiceCollection AddExternalIntegrationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SendEmailOptions>()
            .Bind(configuration.GetSection(SendEmailOptions.EmailOptionsKey))
            .Validate(options => !string.IsNullOrWhiteSpace(options.SmtpClient), "SendEmailOptions:SmtpClient es obligatorio.")
            .Validate(options => options.Port > 0 && options.Port <= 65535, "SendEmailOptions:Port debe estar entre 1 y 65535.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Remitente) && MailAddress.TryCreate(options.Remitente, out _), "SendEmailOptions:Remitente debe ser un correo valido.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Password), "SendEmailOptions:Password es obligatorio.")
            .ValidateOnStart();

        services.AddOptions<EmailValidationOptions>()
            .Bind(configuration.GetSection("EmailValidation"))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConfirmationUrlBase), "EmailValidation:ConfirmationUrlBase es obligatorio.")
            .Validate(options => options.ResendCooldownMinutes > 0, "EmailValidation:ResendCooldownMinutes debe ser mayor que cero.")
            .Validate(options => options.ResendRequestsPerWindow > 0, "EmailValidation:ResendRequestsPerWindow debe ser mayor que cero.")
            .Validate(options => options.ResendWindowMinutes > 0, "EmailValidation:ResendWindowMinutes debe ser mayor que cero.")
            .ValidateOnStart();

        services.AddOptions<ZendeskSendTicketOptions>()
            .Bind(configuration.GetSection(ZendeskSendTicketOptions.ZendeskTicketOptionsKey))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Subdomain) && Uri.TryCreate(options.Subdomain, UriKind.Absolute, out _), "ZendeskSendTicketOptions:Subdomain debe ser una URL absoluta valida.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Email) && MailAddress.TryCreate(options.Email, out _), "ZendeskSendTicketOptions:Email debe ser un correo valido.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ApiToken), "ZendeskSendTicketOptions:ApiToken es obligatorio.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ApiUri), "ZendeskSendTicketOptions:ApiUri es obligatorio.")
            .ValidateOnStart();

        services.AddOptions<NotificationOutboxOptions>()
            .Bind(configuration.GetSection(NotificationOutboxOptions.NotificationOutboxOptionsKey))
            .Validate(options => options.BatchSize > 0, "NotificationOutbox:BatchSize debe ser mayor que cero.")
            .Validate(options => options.IntervalSeconds > 0, "NotificationOutbox:IntervalSeconds debe ser mayor que cero.")
            .Validate(options => options.MaxAttempts > 0, "NotificationOutbox:MaxAttempts debe ser mayor que cero.")
            .Validate(options => options.InitialRetryDelaySeconds > 0, "NotificationOutbox:InitialRetryDelaySeconds debe ser mayor que cero.")
            .Validate(options => options.MaxRetryDelaySeconds >= options.InitialRetryDelaySeconds, "NotificationOutbox:MaxRetryDelaySeconds debe ser mayor o igual al delay inicial.")
            .ValidateOnStart();

        services.AddOptions<PasswordChangeOptions>()
            .Bind(configuration.GetSection(PasswordChangeOptions.PasswordChangeOptionsKey))
            .Validate(options => options.CodeLength >= 4 && options.CodeLength <= 12, "PasswordChange:CodeLength debe estar entre 4 y 12.")
            .Validate(options => options.CodeLifetimeMinutes > 0, "PasswordChange:CodeLifetimeMinutes debe ser mayor que cero.")
            .Validate(options => options.ResendCooldownSeconds >= 0, "PasswordChange:ResendCooldownSeconds no puede ser negativo.")
            .Validate(options => options.MaxFailedAttempts > 0, "PasswordChange:MaxFailedAttempts debe ser mayor que cero.")
            .ValidateOnStart();

        return services;
    }
}
