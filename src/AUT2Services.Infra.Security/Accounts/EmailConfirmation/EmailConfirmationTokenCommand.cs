using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public class EmailConfirmationTokenCommand : Command
{
    public string Email { get; set; } = string.Empty;
    public string ConfirmationToken { get; set; } = string.Empty;
}
