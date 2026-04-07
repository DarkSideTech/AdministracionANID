using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommand : Command
{
    public string? Organizacion { get; set; }
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
    public required HttpContext Context { get; set; }
}