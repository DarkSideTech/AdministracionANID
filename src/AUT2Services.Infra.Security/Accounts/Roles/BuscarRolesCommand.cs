using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class BuscarRolesCommand : Command
{
    public string? Estado { get; set; } = "ACTIVOS";
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
    public required HttpContext Context { get; set; }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new BuscarRolesCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
