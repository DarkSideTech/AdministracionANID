using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Traceability;

public sealed record UsuarioTraceabilityState
{
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public string? NombreADesplegar { get; init; }
    public string? Telefono { get; init; }
    public string? TipoDeUsuario { get; init; }
    public string? EstadoDeUsuario { get; init; }
    public bool? EmailConfirmed { get; init; }
    public bool? Activo { get; init; }
    public bool? UsuarioBase { get; init; }
    public bool? RequiereValidacionEnrrolamiento { get; init; }
    public string? Nacionalidad { get; init; }
    public string? DocumentoDeIdentidad { get; init; }
    public string? NumeroDeDocumento { get; init; }
    public string? CodigoValidadorDocumento { get; init; }
    public string? PrimerNombre { get; init; }
    public string? SegundoNombre { get; init; }
    public string? PrimerApellido { get; init; }
    public string? SegundoApellido { get; init; }
    public string? SexoDeclarativo { get; init; }
    public string? SexoRegistral { get; init; }
    public DateOnly? FechaDeNacimiento { get; init; }
    public string? ValidationToken { get; init; }
    public string? ValidationCode { get; init; }
    public string? NotificationType { get; init; }
    public string? NotificationChannel { get; init; }
    public string? RequestPath { get; init; }
    public string? ActionContext { get; init; }
    public string? Result { get; init; }
    public string? TicketSubject { get; init; }
    public string? TicketPriority { get; init; }
    public DateTimeOffset? ExpiresAtUtc { get; init; }
    public DateTimeOffset? ProcessedAtUtc { get; init; }
    public DateTimeOffset? RespondedAtUtc { get; init; }
    public Guid? ChallengeId { get; init; }

    public static UsuarioTraceabilityState FromUser(Usuario user)
    {
        ArgumentNullException.ThrowIfNull(user);

        var informacion = user.InformacionAdicional.ToInformacionAdicionalModel();
        return new UsuarioTraceabilityState
        {
            UserId = user.Id,
            Email = user.Email,
            NombreADesplegar = user.NombreADesplegar,
            Telefono = user.PhoneNumber,
            TipoDeUsuario = user.TipoDeUsuario,
            EstadoDeUsuario = user.EstadoDeUsuario,
            EmailConfirmed = user.EmailConfirmed,
            Activo = user.Activo,
            UsuarioBase = user.UsuarioBase,
            RequiereValidacionEnrrolamiento = user.RequiereValidacionEnrrolamiento,
            Nacionalidad = informacion.Nacionalidad,
            DocumentoDeIdentidad = informacion.DocumentoDeIdentidad,
            NumeroDeDocumento = informacion.NumeroDeDocumento,
            CodigoValidadorDocumento = informacion.CodigoValidadorDocumento,
            PrimerNombre = informacion.PrimerNombre,
            SegundoNombre = informacion.SegundoNombre,
            PrimerApellido = informacion.PrimerApellido,
            SegundoApellido = informacion.SegundoApellido,
            SexoDeclarativo = informacion.SexoDeclarativo,
            SexoRegistral = informacion.SexoRegistral,
            FechaDeNacimiento = informacion.FechaDeNacimiento
        };
    }
}
