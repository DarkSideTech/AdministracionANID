using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;

public class SolicitaCambioClaveCommand : Command
{
    public string? IdUsuario { get; set; } = string.Empty;
    public string? ClaveActual { get; set; } = string.Empty;
    public string? NuevaClave { get; set; } = string.Empty;
    public string? ConfirmaNuevaClave { get; set; } = string.Empty;
    public HttpRequest Request { get; set; } = default!;
    public HttpResponse Response { get; set; } = default!;

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new SolicitaCambioClaveCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
