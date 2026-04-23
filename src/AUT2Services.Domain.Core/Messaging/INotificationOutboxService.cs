using AUT2Services.Domain.Core.Models;

namespace AUT2Services.Domain.Core.Messaging;

public interface INotificationOutboxService
{
    void QueueEmail(
        EmailDataModel emailData,
        string notificationType,
        string? userId,
        string? requestPath,
        string? deduplicationKey = null);

    void QueueZendeskTicket(
        TicketDataModel ticketData,
        string notificationType,
        string? userId,
        string? requestPath,
        string? deduplicationKey = null);
}
