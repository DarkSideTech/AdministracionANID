using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommand : Command
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Organizacion { get; set; }
}