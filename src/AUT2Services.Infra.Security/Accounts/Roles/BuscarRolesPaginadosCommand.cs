using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class BuscarRolesPaginadosCommand : Command
{
    public int? NumeroDePagina { get; set; } = 1;
    public int? CantidadPorPagina { get; set; } = 10;
    public string? Busqueda { get; set; } = string.Empty;
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
    public required HttpContext Context { get; set; }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new BuscarRolesPaginadosCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
