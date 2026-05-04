using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class BuscarRolesViewModel
{
    [DisplayName("Estado")]
    public string? Estado { get; set; } = "ACTIVOS";
}
