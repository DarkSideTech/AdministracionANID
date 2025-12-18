using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Accounts.Login;

public class LoginCommand : Command
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}