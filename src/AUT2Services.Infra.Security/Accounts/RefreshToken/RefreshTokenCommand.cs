using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Accounts.RefreshToken;

public class RefreshTokenCommand : Command
{
    public string? RefreshToken { get; set; }
}
