namespace AUT2Services.Infra.Security.Interfaces;

public interface IEmailConfirmationThrottleService
{
    bool CanSend(string email, out DateTimeOffset? nextAllowedAtUtc);
}