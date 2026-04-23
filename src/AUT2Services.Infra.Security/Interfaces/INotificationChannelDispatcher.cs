using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.DataTrazabilidad.Persistence;

namespace AUT2Services.Infra.Security.Interfaces;

public interface INotificationChannelDispatcher
{
    string Channel { get; }
    Task<ResultModel> DispatchAsync(NotificationOutboxMessage message, CancellationToken cancellationToken);
}
