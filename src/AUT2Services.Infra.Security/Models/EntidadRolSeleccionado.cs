namespace AUT2Services.Infra.Security.Models;

public sealed class EntidadRolSeleccionado
{
    public string Codigo_UnidadOrganizacional { get; init; } = string.Empty;
    public Guid Id_Entidad { get; init; }
    public Guid Id_Rol { get; init; }
}
