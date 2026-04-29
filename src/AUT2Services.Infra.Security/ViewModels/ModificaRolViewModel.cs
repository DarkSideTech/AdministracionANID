using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class ModificaRolViewModel
{
    [DisplayName("IdRol")]
    public string? IdRol { get; set; } = string.Empty;

    [DisplayName("Descripcion")]
    public string? Descripcion { get; set; } = string.Empty;

    [DisplayName("ValidaEnrrolamiento")]
    public bool? ValidaEnrrolamiento { get; set; } = false;

    [DisplayName("ValidaAsignacionDeRoles")]
    public bool? ValidaAsignacionDeRoles { get; set; } = false;
}
