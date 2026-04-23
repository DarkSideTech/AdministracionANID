using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Accounts.Register;

public class RegisterCommandHandler : CommandHandler,
    IRequestHandler<RegisterCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly INotificationOutboxService notificationOutboxService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ICsrfService csrfService;
    private readonly IEmailConfirmationMessageService emailConfirmationMessageService;
    private readonly JwtOptions jwtOptions;
    private readonly ILogger<RegisterCommandHandler> logger;

    public RegisterCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        IOptions<JwtOptions> jwtOptions,
        INotificationOutboxService notificationOutboxService,
        ISecurityTraceabilityService securityTraceabilityService,
        ICsrfService csrfService,
        IEmailConfirmationMessageService emailConfirmationMessageService,
        ILogger<RegisterCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.notificationOutboxService = notificationOutboxService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.csrfService = csrfService;
        this.emailConfirmationMessageService = emailConfirmationMessageService;
        this.jwtOptions = jwtOptions.Value;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!csrfService.IsRequestValid(command.Request))
        {
            AddError("Invalid CSRF token.");
            return CommandResponse;
        }

        if (command.Contraseña != command.ConfirmaContraseña)
        {
            AddError("La contraseña no corresponde.");
            return CommandResponse;
        }

        var existingUser = await userManager.FindByEmailAsync(command.CorreoElectronico!);
        if (existingUser is not null)
        {
            AddError(existingUser.EmailConfirmed
                ? "El usuario ya existe. Debe iniciar sesion."
                : "El usuario ya existe y su correo electronico esta pendiente de confirmacion. Debe validar el correo o solicitar un nuevo envio.");
            return CommandResponse;
        }

        var requiresEnrollmentValidation = RequiresEnrollmentValidation(command.TipoDeUsuario);
        var userName = string.IsNullOrEmpty(command.NombreUsuario) ? command.CorreoElectronico : command.NombreUsuario;
        var nombreADesplegar = $"{command.PrimerNombre!.Trim()} {command.PrimerApellido!.Trim()}";

        var nuevoUsuario = new Usuario
        {
            UserName = userName,
            NormalizedUserName = userName!.ToUpperInvariant(),
            Email = command.CorreoElectronico,
            NormalizedEmail = command.CorreoElectronico!.ToUpperInvariant(),
            EmailConfirmed = false,
            PhoneNumber = command.NumeroDeTelefono ?? string.Empty,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            AccessFailedCount = jwtOptions.MaximaCantidadIntentosFallidos,
            IdPersona = command.IdPersona,
            NombreADesplegar = nombreADesplegar,
            Descripcion = command.Descripcion,
            TipoDeUsuario = command.TipoDeUsuario,
            Activo = true,
            UsuarioBase = false,
            RequiereValidacionEnrrolamiento = requiresEnrollmentValidation,
            EstadoDeUsuario = requiresEnrollmentValidation
                ? Domain.Enumerations.EnumEstadoDeUsuario.PROCESO_REGISTRO
                : Domain.Enumerations.EnumEstadoDeUsuario.REGISTRADO,
            InformacionAdicional = JsonSerializer.Serialize(new InformacionAdicionalModel()
            {
                Nacionalidad = command.Nacionalidad,
                DocumentoDeIdentidad = command.DocumentoDeIdentidad,
                NumeroDeDocumento = command.NumeroDeDocumento,
                CodigoValidadorDocumento = command.CodigoValidadorDocumento,
                PrimerNombre = command.PrimerNombre,
                SegundoNombre = command.SegundoNombre,
                PrimerApellido = command.PrimerApellido,
                SegundoApellido = command.SegundoApellido,
                SexoDeclarativo = command.SexoDeclarativo,
                SexoRegistral = command.SexoRegistral,
                FechaDeNacimiento = command.FechaDeNacimiento,
                TerminosYCondiciones = command.TerminosYCondiciones
            }),
        };

        var registerMessage = "Usuario registrado. Se programo el envio del correo electronico para confirmar la cuenta.";
        EmailConfirmationDispatch? emailDispatch = null;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var createUserResult = await userManager.CreateAsync(nuevoUsuario, command.Contraseña!);
            if (!createUserResult.Succeeded)
            {
                foreach (var item in createUserResult.Errors)
                {
                    AddError($"{item.Code} - {item.Description}");
                }

                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            var usuarioRegistrado = await userManager.FindByEmailAsync(command.CorreoElectronico!);
            if (usuarioRegistrado is null)
            {
                AddError("El usuario fue creado pero no pudo ser recuperado para continuar el proceso de registro.");
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            emailDispatch = await emailConfirmationMessageService.CreateDispatchAsync(usuarioRegistrado);
            var email = emailConfirmationMessageService.BuildEmailMessage(usuarioRegistrado, emailDispatch);
            notificationOutboxService.QueueEmail(
                email,
                NotificationOutboxNotificationTypes.EmailConfirmation,
                usuarioRegistrado.Id,
                command.Request.Path,
                $"email-confirmation:{usuarioRegistrado.Id}:{emailDispatch.ValidationToken}");
            securityTraceabilityService.TrackCreate(
                command,
                usuarioRegistrado.Id,
                SecurityTraceabilityEventTypes.UsuarioRegistrado,
                UsuarioTraceabilityState.FromUser(usuarioRegistrado) with
                {
                    ActionContext = "REGISTER",
                    RequestPath = command.Request.Path
                });
            securityTraceabilityService.TrackCreate(
                command,
                usuarioRegistrado.Id,
                SecurityTraceabilityEventTypes.CorreoValidacionCuentaProgramado,
                UsuarioTraceabilityState.FromUser(usuarioRegistrado) with
                {
                    ActionContext = "REGISTER",
                    RequestPath = command.Request.Path,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.EmailConfirmation,
                    ValidationToken = emailDispatch.ValidationToken
                });

            if (ShouldNotifyZendesk(command.TipoDeUsuario))
            {
                var ticket = new TicketDataModel()
                {
                    Subject = "Registro Nuevo Usuario Extranjero",
                    Body = $"El usuario {usuarioRegistrado.NombreADesplegar} con la cuenta de correo {usuarioRegistrado.Email} se ha registrado. Se requiere revision del proceso de habilitacion para un usuario extranjero.",
                    Priority = EnumTicketPriority.NORMAL
                };

                notificationOutboxService.QueueZendeskTicket(
                    ticket,
                    NotificationOutboxNotificationTypes.ZendeskForeignUserRegistration,
                    usuarioRegistrado.Id,
                    command.Request.Path,
                    $"zendesk-foreign-user-registration:{usuarioRegistrado.Id}");
                securityTraceabilityService.TrackCreate(
                    command,
                    usuarioRegistrado.Id,
                    SecurityTraceabilityEventTypes.ZendeskRegistroExtranjeroProgramado,
                    UsuarioTraceabilityState.FromUser(usuarioRegistrado) with
                    {
                        ActionContext = "REGISTER",
                        RequestPath = command.Request.Path,
                        NotificationChannel = NotificationOutboxChannels.Zendesk,
                        NotificationType = NotificationOutboxNotificationTypes.ZendeskForeignUserRegistration,
                        TicketSubject = ticket.Subject,
                        TicketPriority = ticket.Priority
                    });
            }

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad del registro del usuario.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);

            CommandResponse.Data = new RegisterResponse(
                Email: usuarioRegistrado.Email ?? command.CorreoElectronico ?? string.Empty,
                RequiresEmailConfirmation: true,
                CanResendConfirmationEmail: true,
                Message: registerMessage,
                ConfirmationUrl: emailDispatch.ManualConfirmationUrl,
                ValidationToken: emailConfirmationMessageService.GetValidationTokenForResponse(emailDispatch)
            );
            CommandResponse.Result = true;
            return CommandResponse;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(
                ex,
                "No fue posible completar el registro y encolar las notificaciones para el usuario {Email}.",
                command.CorreoElectronico);
            AddError("No fue posible completar el registro en este momento. Intenta nuevamente.");
            return CommandResponse;
        }
    }

    private static bool RequiresEnrollmentValidation(string? tipoDeUsuario)
    {
        return ShouldNotifyZendesk(tipoDeUsuario);
    }

    private static bool ShouldNotifyZendesk(string? tipoDeUsuario)
    {
        return string.Equals(tipoDeUsuario, Domain.Enumerations.EnumTipoDeUsuario.EXTRANJERO, StringComparison.Ordinal) ||
               string.Equals(tipoDeUsuario, Domain.Enumerations.EnumTipoDeUsuario.EXTRANJERO_RESIDENTE, StringComparison.Ordinal) ||
               string.Equals(tipoDeUsuario, Domain.Enumerations.EnumTipoDeUsuario.EXTRANJERO_NACIONALIZADO, StringComparison.Ordinal);
    }
}
