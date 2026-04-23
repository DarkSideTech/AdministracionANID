using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.ResendEmailConfirmationToken;

public class ResendEmailConfirmationTokenCommandHandler : CommandHandler,
    IRequestHandler<ResendEmailConfirmationTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly ICsrfService csrfService;
    private readonly IEmailConfirmationThrottleService emailConfirmationThrottleService;
    private readonly INotificationOutboxService notificationOutboxService;
    private readonly IEmailConfirmationMessageService emailConfirmationMessageService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ILogger<ResendEmailConfirmationTokenCommandHandler> logger;

    public ResendEmailConfirmationTokenCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        ICsrfService csrfService,
        IEmailConfirmationThrottleService emailConfirmationThrottleService,
        INotificationOutboxService notificationOutboxService,
        IEmailConfirmationMessageService emailConfirmationMessageService,
        ISecurityTraceabilityService securityTraceabilityService,
        ILogger<ResendEmailConfirmationTokenCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.csrfService = csrfService;
        this.emailConfirmationThrottleService = emailConfirmationThrottleService;
        this.notificationOutboxService = notificationOutboxService;
        this.emailConfirmationMessageService = emailConfirmationMessageService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ResendEmailConfirmationTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Data = new EmailConfirmationDispatchResponse(
            Email: command.Email.Trim(),
            Message: "Si la cuenta existe y el correo electronico esta pendiente de confirmacion, se enviara un nuevo correo electronico de confirmacion.",
            ConfirmationUrl: null,
            ValidationToken: null);
        CommandResponse.Result = true;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!csrfService.IsRequestValid(command.Request))
        {
            AddError("Invalid CSRF token.");
            return CommandResponse;
        }

        var normalizedEmail = command.Email.Trim();

        var usuario = await userManager.FindByEmailAsync(normalizedEmail);
        if (usuario is null || usuario.EmailConfirmed)
        {
            return CommandResponse;
        }

        if (!emailConfirmationThrottleService.CanSend(normalizedEmail, out _))
        {
            return CommandResponse;
        }

        try
        {
            var emailDispatch = await emailConfirmationMessageService.CreateDispatchAsync(usuario);
            var email = emailConfirmationMessageService.BuildEmailMessage(usuario, emailDispatch);
            notificationOutboxService.QueueEmail(
                email,
                NotificationOutboxNotificationTypes.EmailConfirmation,
                usuario.Id,
                command.Request.Path,
                $"email-confirmation-resend:{usuario.Id}:{emailDispatch.ValidationToken}");
            securityTraceabilityService.TrackCreate(
                command,
                usuario.Id,
                SecurityTraceabilityEventTypes.CorreoValidacionCuentaProgramado,
                UsuarioTraceabilityState.FromUser(usuario) with
                {
                    ActionContext = "RESEND",
                    RequestPath = command.Request.Path,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.EmailConfirmation,
                    ValidationToken = emailDispatch.ValidationToken
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad del reenvio de confirmacion.");
                return CommandResponse;
            }

            CommandResponse.Data = new EmailConfirmationDispatchResponse(
                Email: usuario.Email ?? normalizedEmail,
                Message: "Se ha programado un nuevo correo electronico de confirmacion.",
                ConfirmationUrl: emailDispatch.ManualConfirmationUrl,
                ValidationToken: emailConfirmationMessageService.GetValidationTokenForResponse(emailDispatch));
            CommandResponse.Result = true;
            return CommandResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error encolando el reenvio de correo de confirmacion para {Email}.",
                normalizedEmail);
            AddError("No fue posible reenviar el correo electronico de confirmacion.");
            return CommandResponse;
        }
    }
}
