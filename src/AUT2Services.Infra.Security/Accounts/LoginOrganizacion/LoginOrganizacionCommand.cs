using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommand : Command
{
    public string? Organizacion { get; set; }
}