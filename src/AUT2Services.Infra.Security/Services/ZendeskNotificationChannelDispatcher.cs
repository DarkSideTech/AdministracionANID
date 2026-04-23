using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Services;

public sealed class ZendeskNotificationChannelDispatcher(
    ITicketDataSender ticketDataSender,
    ILogger<ZendeskNotificationChannelDispatcher> logger) : INotificationChannelDispatcher
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly ITicketDataSender ticketDataSender = ticketDataSender;
    private readonly ILogger<ZendeskNotificationChannelDispatcher> logger = logger;

    public string Channel => NotificationOutboxChannels.Zendesk;

    public async Task<ResultModel> DispatchAsync(NotificationOutboxMessage message, CancellationToken cancellationToken)
    {
        TicketDataModel? payload;
        try
        {
            payload = JsonSerializer.Deserialize<TicketDataModel>(message.PayloadJson, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible deserializar el payload del mensaje Zendesk {NotificationOutboxId}.", message.Id);
            return new ResultModel
            {
                Result = false,
                Data = "Payload invalido para Zendesk."
            };
        }

        if (payload is null)
        {
            return new ResultModel
            {
                Result = false,
                Data = "Payload vacio para Zendesk."
            };
        }

        return await ticketDataSender.SendTicketData(payload);
    }
}
