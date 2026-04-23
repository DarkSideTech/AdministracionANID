using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.EmailConfirmationToken;

public class EmailConfirmationTokenCommand : Command
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
}
