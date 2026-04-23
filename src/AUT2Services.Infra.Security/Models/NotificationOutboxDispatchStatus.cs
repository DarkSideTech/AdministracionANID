namespace AUT2Services.Infra.Security.Models;

public static class NotificationOutboxDispatchStatus
{
    public const short Pending = 0;
    public const short Processed = 1;
    public const short DeadLetter = 2;
}
