using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class LoginOrganizacionViewModel
{
    [DisplayName("Organizacion")]
    public string? Organizacion { get; set; }
}