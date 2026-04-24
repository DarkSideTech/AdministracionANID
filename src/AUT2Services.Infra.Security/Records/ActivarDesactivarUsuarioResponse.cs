namespace AUT2Services.Infra.Security.Records;

public sealed record ActivarDesactivarUsuarioResponse(
    string? IdUsuario,
    string? Message
);
