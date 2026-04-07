using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.CurrentUser;

public class CurrentUserCommand : Command
{
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
    public required HttpContext Context { get; set; }
}