namespace AUT2Services.Infra.Security.Records;

public sealed record ProcesoActivo (
    string? Codigo,
    string? NombreProceso,
    IList<string>? Roles,
    string? NivelDeProceso,
    string? Url,
    string? Token,
    string? ComoDesplegarUrlDeProceso
);
