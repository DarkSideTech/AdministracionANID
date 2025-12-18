using AUT2Services.Domain.Core.Commands;
using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Accounts.Register;

public class RegisterCommand : Command
{
    public string? NombreUsuario { get; set; } = string.Empty;
    public string? NombreUsuarioNormalizado { get; set; } = string.Empty;
    public string? CorreoElectronico { get; set; } = string.Empty;
    public string? CorreoElectronicoNormalizado { get; set; } = string.Empty;
    public string? CorreoElectronicoConfirmado { get; set; } = string.Empty;
    public string? NumeroDeTelefono { get; set; } = string.Empty;
    public string? NumeroDeTelefonoConfirmado { get; set; } = string.Empty;
    public bool? DobleFactorHabilitado { get; set; } = false;
    public int? CantidadDeAccesosFallidos { get; set; } = 5;
    public string? IdPersona { get; set; } = string.Empty;
    public string? NombreADesplegar { get; set; } = string.Empty;
    public string? Descripcion { get; set; } = string.Empty;
    public string? TipoDeUsuario { get; set; } = string.Empty;
    public bool? Activo { get; set; } = true;
    public bool? UsuarioBase { get; set; } = false;
    public bool? RequiereValidacionEnrrolamiento { get; set; } = false;
    public string? EstadoDeUsuario { get; set; } = string.Empty;
    public string? Contraseña { get; set; } = string.Empty;
    public string? Nacionalidad { get; set; } = string.Empty;
    public string? DocumentoDeIdentidad { get; set; } = string.Empty;
    public string? NumeroDeDocumento { get; set; } = string.Empty;
    public string? CodigoValidadorDocumento { get; set; } = string.Empty;
    public string? PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; } = string.Empty;
    public string? PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; } = string.Empty;
    public string? SexoDeclarativo { get; set; } = string.Empty;
    public string? SexoRegistral { get; set; } = string.Empty;
    public DateTime? FechaDeNacimiento { get; set; } = DateTime.MinValue;
    public bool? TerminosYCondiciones { get; set; } = true;

}
