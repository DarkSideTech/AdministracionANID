using Microsoft.AspNetCore.Identity;

namespace AUT2Services.Domain.Security.Entities;

public class Rol : IdentityRole
{
    public string? Descripcion { get; set; } = string.Empty;
    public bool? RequiereAccionParaSerAsignado { get; set; } = false;
    public bool? ActivaDetalleDeAutorizaciones { get; set; } = false;
    public bool? RequiereValidacionDeAsignacion { get; set; } = false;
    public bool? ValidaAsignacionDeRoles { get; set; } = false;
    public bool? ValidaEnrrolamiento { get; set; } = false;
    public bool? RolBase { get; set; } = false;
    public bool? Activo { get; set; } = true;
}

