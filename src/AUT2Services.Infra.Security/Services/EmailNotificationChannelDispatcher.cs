using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Services;

public sealed class EmailNotificationChannelDispatcher(
    IEmailMessageSender emailMessageSender,
    ILogger<EmailNotificationChannelDispatcher> logger) : INotificationChannelDispatcher
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly IEmailMessageSender emailMessageSender = emailMessageSender;
    private readonly ILogger<EmailNotificationChannelDispatcher> logger = logger;

    public string Channel => NotificationOutboxChannels.Email;

    public async Task<ResultModel> DispatchAsync(NotificationOutboxMessage message, CancellationToken cancellationToken)
    {
        EmailDataModel? payload;
        try
        {
            payload = JsonSerializer.Deserialize<EmailDataModel>(message.PayloadJson, JsonSerializerOptions);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible deserializar el payload del mensaje email {NotificationOutboxId}.", message.Id);
            return new ResultModel
            {
                Result = false,
                Data = "Payload invalido para correo."
            };
        }

        if (payload is null)
        {
            return new ResultModel
            {
                Result = false,
                Data = "Payload vacio para correo."
            };
        }

        return await emailMessageSender.SendEmail(payload);
    }
}
