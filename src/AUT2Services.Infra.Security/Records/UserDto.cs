namespace AUT2Services.Infra.Security.Records;

public sealed record UserDto(
    string? Id,
    string? Email,
    string? NombreADesplegar,
    string? NumeroDeTelefono,
    string? TipoDeUsuario,
    string? Nacionalidad,
    string? DocumentoDeIdentidad,
    string? NumeroDeDocumento,
    string? CodigoValidadorDocumento,
    string? PrimerNombre,
    string? SegundoNombre,
    string? PrimerApellido,
    string? SegundoApellido,
    string? SexoDeclarativo,
    string? SexoRegistral,
    DateOnly? FechaDeNacimiento,
    IList<string>? Roles,
    IList<string>? UnidadesOrganizacionales
);
