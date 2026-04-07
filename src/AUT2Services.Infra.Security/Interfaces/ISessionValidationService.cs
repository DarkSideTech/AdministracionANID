namespace AUT2Services.Infra.Security.Interfaces;

public interface ISessionValidationService
{
    Task<bool> IsSessionValidAsync(string? userId, string? sessionId, string? securityStamp, CancellationToken cancellationToken = default);
}
