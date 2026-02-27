using AUT2Services.Domain.Core.Models;

namespace AUT2Services.Domain.Core.Messaging;

public interface IEmailMessageSender
{
    Task<ResultModel> SendEmail(EmailDataModel emailData);
}