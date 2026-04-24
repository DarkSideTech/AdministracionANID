using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.ActivarUsuario;

public class ActivarUsuarioCommand : Command
{
    public string? IdUsuario { get; set; } = string.Empty;
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ActivarUsuarioCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
