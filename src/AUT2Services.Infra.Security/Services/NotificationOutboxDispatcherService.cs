using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Services;

public sealed class NotificationOutboxDispatcherService : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly IOptions<NotificationOutboxOptions> options;
    private readonly IClock clock;
    private readonly ILogger<NotificationOutboxDispatcherService> logger;

    public NotificationOutboxDispatcherService(
        IServiceScopeFactory scopeFactory,
        IOptions<NotificationOutboxOptions> options,
        IClock clock,
        ILogger<NotificationOutboxDispatcherService> logger)
    {
        this.scopeFactory = scopeFactory;
        this.options = options;
        this.clock = clock;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var currentOptions = options.Value;
        if (!currentOptions.Enabled)
        {
            logger.LogInformation("Notification outbox dispatcher disabled.");
            return;
        }

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(1, currentOptions.IntervalSeconds)));

        await DispatchPendingNotificationsAsync(stoppingToken).ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            await DispatchPendingNotificationsAsync(stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task DispatchPendingNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AUT2ServicesContext>();
        var dispatchers = scope.ServiceProvider.GetServices<INotificationChannelDispatcher>()
            .ToDictionary(dispatcher => dispatcher.Channel, StringComparer.OrdinalIgnoreCase);
        var securityTraceabilityService = scope.ServiceProvider.GetRequiredService<ISecurityTraceabilityService>();
        var currentOptions = options.Value;
        var now = clock.UtcNow;

        var pendingMessages = await dbContext.NotificationOutboxMessages
            .AsTracking()
            .Where(message => message.DispatchStatus == NotificationOutboxDispatchStatus.Pending && message.NextAttemptUtc <= now)
            .OrderBy(message => message.NextAttemptUtc)
            .ThenBy(message => message.CreatedAtUtc)
            .ThenBy(message => message.Id)
            .Take(Math.Max(1, currentOptions.BatchSize))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (pendingMessages.Count == 0)
        {
            return;
        }

        foreach (var pendingMessage in pendingMessages)
        {
            var attemptAtUtc = clock.UtcNow;

            if (!dispatchers.TryGetValue(pendingMessage.Channel, out var dispatcher))
            {
                logger.LogError(
                    "No existe dispatcher para el canal {Channel} del mensaje {NotificationOutboxId}.",
                    pendingMessage.Channel,
                    pendingMessage.Id);

                MarkFailure(pendingMessage, currentOptions, attemptAtUtc, "No existe dispatcher para el canal configurado.");
                continue;
            }

            try
            {
                var result = await dispatcher.DispatchAsync(pendingMessage, cancellationToken).ConfigureAwait(false);
                if (result.Result)
                {
                    pendingMessage.DispatchStatus = NotificationOutboxDispatchStatus.Processed;
                    pendingMessage.DispatchAttempts += 1;
                    pendingMessage.LastDispatchAttemptUtc = attemptAtUtc;
                    pendingMessage.DispatchedAtUtc = attemptAtUtc;
                    pendingMessage.LastError = null;
                    dbContext.NotificationOutboxMessages.Update(pendingMessage);
                    TrackDispatchTraceability(securityTraceabilityService, pendingMessage, attemptAtUtc);
                    continue;
                }

                MarkFailure(
                    pendingMessage,
                    currentOptions,
                    attemptAtUtc,
                    string.IsNullOrWhiteSpace(result.Data?.ToString())
                        ? "Error de despacho sin detalle."
                        : result.Data!.ToString()!);
                dbContext.NotificationOutboxMessages.Update(pendingMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error despachando el mensaje {NotificationOutboxId} del canal {Channel}.",
                    pendingMessage.Id,
                    pendingMessage.Channel);

                MarkFailure(pendingMessage, currentOptions, attemptAtUtc, ex.Message);
                dbContext.NotificationOutboxMessages.Update(pendingMessage);
            }
        }

        try
        {
            if (!await dbContext.Commit().ConfigureAwait(false))
            {
                logger.LogWarning("No fue posible persistir el lote procesado del notification outbox.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible persistir el lote procesado del notification outbox.");
        }
    }

    private static void MarkFailure(
        NotificationOutboxMessage message,
        NotificationOutboxOptions options,
        DateTimeOffset attemptAtUtc,
        string error)
    {
        message.DispatchAttempts += 1;
        message.LastDispatchAttemptUtc = attemptAtUtc;
        message.LastError = error;

        if (message.DispatchAttempts >= options.MaxAttempts)
        {
            message.DispatchStatus = NotificationOutboxDispatchStatus.DeadLetter;
            return;
        }

        var retryDelaySeconds = options.InitialRetryDelaySeconds * (int)Math.Pow(2, Math.Max(0, message.DispatchAttempts - 1));
        retryDelaySeconds = Math.Min(retryDelaySeconds, options.MaxRetryDelaySeconds);
        message.NextAttemptUtc = attemptAtUtc.AddSeconds(Math.Max(1, retryDelaySeconds));
    }

    private static void TrackDispatchTraceability(
        ISecurityTraceabilityService securityTraceabilityService,
        NotificationOutboxMessage message,
        DateTimeOffset processedAtUtc)
    {
        var eventType = ResolveTraceabilityEventType(message);
        if (eventType is null || string.IsNullOrWhiteSpace(message.UserId))
        {
            return;
        }

        securityTraceabilityService.TrackCreate(
            SecurityTraceabilityCommandTypes.NotificationOutboxDispatch,
            message.UserId,
            eventType,
            BuildTraceabilityState(message, processedAtUtc));
    }

    private static UsuarioTraceabilityState BuildTraceabilityState(
        NotificationOutboxMessage message,
        DateTimeOffset processedAtUtc)
    {
        if (string.Equals(message.Channel, NotificationOutboxChannels.Email, StringComparison.OrdinalIgnoreCase))
        {
            var email = TryDeserialize<EmailDataModel>(message.PayloadJson);
            return new UsuarioTraceabilityState
            {
                UserId = message.UserId,
                Email = email?.ToMailboxAddresses.FirstOrDefault()?.Address,
                NotificationType = message.NotificationType,
                NotificationChannel = message.Channel,
                RequestPath = message.RequestPath,
                ActionContext = ResolveEmailActionContext(message),
                ValidationToken = email?.ValidationToken ?? ExtractValidationToken(message.DeduplicationKey),
                ProcessedAtUtc = processedAtUtc,
                Result = "PROCESSED"
            };
        }

        if (string.Equals(message.Channel, NotificationOutboxChannels.Zendesk, StringComparison.OrdinalIgnoreCase))
        {
            var ticket = TryDeserialize<TicketDataModel>(message.PayloadJson);
            return new UsuarioTraceabilityState
            {
                UserId = message.UserId,
                NotificationType = message.NotificationType,
                NotificationChannel = message.Channel,
                RequestPath = message.RequestPath,
                ActionContext = "REGISTER_FOREIGN_USER",
                TicketSubject = ticket?.Subject,
                TicketPriority = ticket?.Priority,
                ProcessedAtUtc = processedAtUtc,
                Result = "PROCESSED"
            };
        }

        return new UsuarioTraceabilityState
        {
            UserId = message.UserId,
            NotificationType = message.NotificationType,
            NotificationChannel = message.Channel,
            RequestPath = message.RequestPath,
            ProcessedAtUtc = processedAtUtc,
            Result = "PROCESSED"
        };
    }

    private static string? ResolveTraceabilityEventType(NotificationOutboxMessage message)
    {
        return message.NotificationType switch
        {
            NotificationOutboxNotificationTypes.EmailConfirmation
                when message.DeduplicationKey?.StartsWith("email-change-confirmation:", StringComparison.OrdinalIgnoreCase) == true
                => SecurityTraceabilityEventTypes.CorreoValidacionCambioCorreoEnviado,
            NotificationOutboxNotificationTypes.EmailConfirmation
                => SecurityTraceabilityEventTypes.CorreoValidacionCuentaEnviado,
            NotificationOutboxNotificationTypes.PasswordChangeVerificationCode
                => SecurityTraceabilityEventTypes.CodigoValidacionCambioClaveEnviado,
            NotificationOutboxNotificationTypes.ZendeskForeignUserRegistration
                => SecurityTraceabilityEventTypes.ZendeskRegistroExtranjeroEnviado,
            _ => null
        };
    }

    private static string ResolveEmailActionContext(NotificationOutboxMessage message)
    {
        if (message.DeduplicationKey?.StartsWith("email-change-confirmation:", StringComparison.OrdinalIgnoreCase) == true)
        {
            return "EMAIL_CHANGE";
        }

        if (message.DeduplicationKey?.StartsWith("email-confirmation-resend:", StringComparison.OrdinalIgnoreCase) == true)
        {
            return "RESEND";
        }

        if (message.DeduplicationKey?.StartsWith("email-confirmation:", StringComparison.OrdinalIgnoreCase) == true)
        {
            return "REGISTER";
        }

        if (message.DeduplicationKey?.StartsWith("password-change:", StringComparison.OrdinalIgnoreCase) == true)
        {
            return "PASSWORD_CHANGE";
        }

        return "OUTBOX";
    }

    private static string? ExtractValidationToken(string? deduplicationKey)
    {
        if (string.IsNullOrWhiteSpace(deduplicationKey))
        {
            return null;
        }

        var parts = deduplicationKey.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Length < 3 ? null : parts[^1];
    }

    private static TPayload? TryDeserialize<TPayload>(string payloadJson)
    {
        try
        {
            return JsonSerializer.Deserialize<TPayload>(payloadJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }
        catch
        {
            return default;
        }
    }
}
