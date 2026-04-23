using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Services;

public sealed class NotificationOutboxService(
    AUT2ServicesContext dbContext,
    IClock clock) : INotificationOutboxService
{
    private const int MaxDeduplicationKeyLength = 300;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly AUT2ServicesContext dbContext = dbContext;
    private readonly IClock clock = clock;

    public void QueueEmail(
        EmailDataModel emailData,
        string notificationType,
        string? userId,
        string? requestPath,
        string? deduplicationKey = null)
    {
        Queue(
            channel: NotificationOutboxChannels.Email,
            notificationType: notificationType,
            payloadJson: JsonSerializer.Serialize(emailData, JsonSerializerOptions),
            userId: userId,
            requestPath: requestPath,
            deduplicationKey: deduplicationKey);
    }

    public void QueueZendeskTicket(
        TicketDataModel ticketData,
        string notificationType,
        string? userId,
        string? requestPath,
        string? deduplicationKey = null)
    {
        Queue(
            channel: NotificationOutboxChannels.Zendesk,
            notificationType: notificationType,
            payloadJson: JsonSerializer.Serialize(ticketData, JsonSerializerOptions),
            userId: userId,
            requestPath: requestPath,
            deduplicationKey: deduplicationKey);
    }

    private void Queue(
        string channel,
        string notificationType,
        string payloadJson,
        string? userId,
        string? requestPath,
        string? deduplicationKey)
    {
        var now = clock.UtcNow;

        dbContext.NotificationOutboxMessages.Add(new NotificationOutboxMessage
        {
            Id = Guid.NewGuid(),
            Channel = channel,
            NotificationType = notificationType,
            UserId = userId,
            RequestPath = string.IsNullOrWhiteSpace(requestPath) ? null : requestPath,
            DeduplicationKey = NormalizeDeduplicationKey(deduplicationKey),
            PayloadJson = payloadJson,
            DispatchStatus = NotificationOutboxDispatchStatus.Pending,
            DispatchAttempts = 0,
            CreatedAtUtc = now,
            NextAttemptUtc = now,
            SchemaVersion = 1
        });
    }

    private static string? NormalizeDeduplicationKey(string? deduplicationKey)
    {
        if (string.IsNullOrWhiteSpace(deduplicationKey))
        {
            return null;
        }

        var normalizedKey = deduplicationKey.Trim();
        if (normalizedKey.Length <= MaxDeduplicationKeyLength)
        {
            return normalizedKey;
        }

        var lastSeparatorIndex = normalizedKey.LastIndexOf(':');
        if (lastSeparatorIndex > 0 && lastSeparatorIndex < normalizedKey.Length - 1)
        {
            var prefix = normalizedKey[..(lastSeparatorIndex + 1)];
            var suffix = normalizedKey[(lastSeparatorIndex + 1)..];
            var shortenedKey = $"{prefix}sha256-{ComputeHash(suffix)}";

            if (shortenedKey.Length <= MaxDeduplicationKeyLength)
            {
                return shortenedKey;
            }
        }

        return $"sha256-{ComputeHash(normalizedKey)}";
    }

    private static string ComputeHash(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash);
    }
}
