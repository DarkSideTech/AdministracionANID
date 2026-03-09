using AUT2Services.Domain.Core.Models;

namespace AUT2Services.Domain.Core.Messaging;

public interface ITicketDataSender
{
    Task<ResultModel> SendTicketData(TicketDataModel ticketDataModel);
}