namespace AUT2Services.Infra.Security.ViewModels;

public class RegisterCommandViewModel
{
    public string? CorreoElectronico { get; set; } = string.Empty;
    public string? Nacionalidad { get; set; } = string.Empty;
    public string? TipoDeUsuario { get; set; } = string.Empty;
    public string? DocumentoDeIdentidad { get; set; } = string.Empty;
    public string? NumeroDeDocumento { get; set; } = string.Empty;
    public string? CodigoValidadorDocumento { get; set; } = string.Empty;
    public string? PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; } = string.Empty;
    public string? PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; } = string.Empty;
    public string? SexoDeclarativo { get; set; } = string.Empty;
    public string? SexoRegistral { get; set; } = string.Empty;
    public DateOnly? FechaDeNacimiento { get; set; }
    public string? Contraseña { get; set; } = string.Empty;
    public bool? TerminosYCondiciones { get; set; } = true;

}
