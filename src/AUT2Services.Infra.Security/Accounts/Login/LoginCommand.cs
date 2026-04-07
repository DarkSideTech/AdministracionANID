using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.Login;

public class LoginCommand : Command
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
}