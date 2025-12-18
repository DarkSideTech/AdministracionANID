namespace AUT2Services.Infra.Security.Models;

public class InformacionAdicionalModel
{
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
