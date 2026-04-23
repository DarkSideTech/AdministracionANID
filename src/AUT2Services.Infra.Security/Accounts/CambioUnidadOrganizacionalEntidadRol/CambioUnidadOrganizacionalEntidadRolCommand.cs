using AUT2Services.Domain.Core.Commands;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;

public class CambioUnidadOrganizacionalEntidadRolCommand : Command
{
    public string? Id_Entidad { get; set; }
    public string? Id_Rol { get; set; }
    public required HttpRequest Request { get; set; }
    public required HttpResponse Response { get; set; }
    public required HttpContext Context { get; set; }
}
