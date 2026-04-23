using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;

public class ModificaCorreoElectronicoCommand : Command
{
    public string? IdUsuario { get; set; } = string.Empty;
    public string? NuevoCorreoElectronico { get; set; } = string.Empty;
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ModificaCorreoElectronicoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
