using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;

public class ReenviaCodigoCambioClaveCommand : Command
{
    public string? IdUsuario { get; set; } = string.Empty;
    public HttpRequest Request { get; set; } = default!;
    public HttpResponse Response { get; set; } = default!;

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ReenviaCodigoCambioClaveCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
