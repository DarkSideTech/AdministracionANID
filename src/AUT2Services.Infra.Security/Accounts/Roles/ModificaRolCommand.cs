using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ModificaRolCommand : Command
{
    public string? IdRol { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public bool? ValidaEnrrolamiento { get; set; } = false;
    public bool? ValidaAsignacionDeRoles { get; set; } = false;
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ModificaRolCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
