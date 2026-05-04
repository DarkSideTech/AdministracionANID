namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public sealed record UsuarioPendienteEnrrolamientoViewModel(
    string? IdUsuario,
    string? CorreoElectronico,
    string? NombreADesplegar,
    string? TipoDeUsuario,
    string? EstadoDeUsuario,
    bool? RequiereValidacionEnrrolamiento,
    bool CorreoElectronicoConfirmado,
    string? Nacionalidad,
    string? DocumentoDeIdentidad,
    string? NumeroDeDocumento,
    string? CodigoValidadorDocumento,
    string? PrimerNombre,
    string? SegundoNombre,
    string? PrimerApellido,
    string? SegundoApellido,
    DateOnly? FechaDeNacimiento);
