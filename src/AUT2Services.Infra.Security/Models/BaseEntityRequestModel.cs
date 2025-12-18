namespace AUT2Services.Infra.Security.Models;

public class BaseEntityRequestModel
{
    public Guid Id_Entidad { get; set; }
    public Guid Id_UnidadOrganizacional { get; set; }
    public string NombreOrganizacion { get; set; }
}
