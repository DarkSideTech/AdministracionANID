using System.ComponentModel;

namespace AUT2Services.Infra.Security.ViewModels;

public class RegisterViewModel
{
    [DisplayName("CorreoElectronico")]
    public string? CorreoElectronico { get; set; } = string.Empty;

    [DisplayName("Nacionalidad")]
    public string? Nacionalidad { get; set; } = string.Empty;

    [DisplayName("TipoDeUsuario")]
    public string? TipoDeUsuario { get; set; } = string.Empty;

    [DisplayName("DocumentoDeIdentidad")]
    public string? DocumentoDeIdentidad { get; set; } = string.Empty;

    [DisplayName("NumeroDeDocumento")]
    public string? NumeroDeDocumento { get; set; } = string.Empty;

    [DisplayName("CodigoValidadorDocumento")]
    public string? CodigoValidadorDocumento { get; set; } = string.Empty;

    [DisplayName("PrimerNombre")]
    public string? PrimerNombre { get; set; } = string.Empty;

    [DisplayName("SegundoNombre")]
    public string? SegundoNombre { get; set; } = string.Empty;

    [DisplayName("PrimerApellido")]
    public string? PrimerApellido { get; set; } = string.Empty;

    [DisplayName("SegundoApellido")]
    public string? SegundoApellido { get; set; } = string.Empty;

    [DisplayName("SexoDeclarativo")]
    public string? SexoDeclarativo { get; set; } = string.Empty;

    [DisplayName("SexoRegistral")]
    public string? SexoRegistral { get; set; } = string.Empty;

    [DisplayName("FechaDeNacimiento")]
    public DateTime? FechaDeNacimiento { get; set; } = DateTime.MinValue;

    [DisplayName("Contraseña")]
    public string? Contraseña { get; set; } = string.Empty;

    [DisplayName("TerminosYCondiciones")]
    public bool? TerminosYCondiciones { get; set; } = true;
}