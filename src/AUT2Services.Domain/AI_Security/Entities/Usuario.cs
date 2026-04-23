using Microsoft.AspNetCore.Identity;

namespace AUT2Services.Domain.Security.Entities;

public class Usuario : IdentityUser
{
    public string? IdPersona { get; set; } = string.Empty;
    public string? NombreADesplegar { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public string? TipoDeUsuario { get; set; } = string.Empty;
    public bool? Activo { get; set; } = true;
    public bool? UsuarioBase { get; set; } = false;
    public bool? RequiereValidacionEnrrolamiento { get; set; } = false;
    public string? EstadoDeUsuario { get; set; } = string.Empty;
    public string? InformacionAdicional { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTimeOffset? RefreshTokenExpiresAtUtc { get; set; }

    public List<RefreshToken> RefreshTokens { get; set; } = [];
}

