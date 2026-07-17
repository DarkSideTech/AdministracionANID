using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Interfaces;

public interface IClaveUnicaClient
{
    bool IsConfigured { get; }

    Task<string> ExchangeCodeAsync(string code, CancellationToken cancellationToken);

    Task<ClaveUnicaUserInfoResponse> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken);
}
