using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;

public class ModificaCorreoElectronicoCommandHandler : CommandHandler,
    IRequestHandler<ModificaCorreoElectronicoCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly INotificationOutboxService notificationOutboxService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ICsrfService csrfService;
    private readonly IEmailConfirmationMessageService emailConfirmationMessageService;
    private readonly ICurrentUserService currentUserService;
    private readonly ISessionValidationService sessionValidationService;
    private readonly ITokenService tokenService;
    private readonly IAuthCookieService authCookieService;
    private readonly ILogger<ModificaCorreoElectronicoCommandHandler> logger;

    public ModificaCorreoElectronicoCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        INotificationOutboxService notificationOutboxService,
        ISecurityTraceabilityService securityTraceabilityService,
        ICsrfService csrfService,
        IEmailConfirmationMessageService emailConfirmationMessageService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        ITokenService tokenService,
        IAuthCookieService authCookieService,
        ILogger<ModificaCorreoElectronicoCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.notificationOutboxService = notificationOutboxService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.csrfService = csrfService;
        this.emailConfirmationMessageService = emailConfirmationMessageService;
        this.currentUserService = currentUserService;
        this.sessionValidationService = sessionValidationService;
        this.tokenService = tokenService;
        this.authCookieService = authCookieService;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ModificaCorreoElectronicoCommand command, CancellationToken cancellationToken)
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

        if (!currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(currentUserService.UserId))
        {
            AddError("Usuario no autorizado.");
            return CommandResponse;
        }

        var principal = currentUserService.GetClaimsPrincipal(cancellationToken);
        var securityStamp = principal?.FindFirst(EnumTokenValidationClaims.SecurityStamp)?.Value;
        var isSessionValid = await sessionValidationService.IsSessionValidAsync(
            currentUserService.UserId,
            currentUserService.SessionId,
            securityStamp,
            cancellationToken);

        if (!isSessionValid)
        {
            AddError("La sesion del usuario no es valida.");
            return CommandResponse;
        }

        if (!string.Equals(currentUserService.UserId, command.IdUsuario, StringComparison.Ordinal))
        {
            AddError("No tienes permisos para modificar este usuario.");
            return CommandResponse;
        }

        var existingUser = await userManager.FindByIdAsync(command.IdUsuario!);
        if (existingUser is null)
        {
            AddError("El usuario no existe.");
            return CommandResponse;
        }

        if (!existingUser.EmailConfirmed)
        {
            AddError("El usuario tiene la validacion pendiente del correo electronico.");
            return CommandResponse;
        }

        if (existingUser.Activo != true)
        {
            AddError("El usuario no se encuentra activo.");
            return CommandResponse;
        }

        if (!string.Equals(existingUser.EstadoDeUsuario, EnumEstadoDeUsuario.REGISTRADO, StringComparison.OrdinalIgnoreCase))
        {
            AddError("El usuario no se encuentra en estado REGISTRADO.");
            return CommandResponse;
        }

        if (existingUser.UsuarioBase == true)
        {
            AddError("El usuario base no puede ser modificado.");
            return CommandResponse;
        }

        var nuevoCorreoElectronico = command.NuevoCorreoElectronico!.Trim();
        if (string.Equals(existingUser.Email?.Trim(), nuevoCorreoElectronico, StringComparison.OrdinalIgnoreCase))
        {
            AddError("El nuevo correo electronico debe ser distinto al correo actual.");
            return CommandResponse;
        }

        var duplicatedUser = await userManager.FindByEmailAsync(nuevoCorreoElectronico);
        if (duplicatedUser is not null && !string.Equals(duplicatedUser.Id, existingUser.Id, StringComparison.Ordinal))
        {
            AddError(duplicatedUser.EmailConfirmed
                ? "El correo electronico ingresado ya esta en uso."
                : "El correo electronico ingresado ya esta en uso y se encuentra pendiente de confirmacion.");
            return CommandResponse;
        }

        EmailConfirmationDispatch? emailDispatch = null;
        var beforeState = UsuarioTraceabilityState.FromUser(existingUser);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            existingUser.Email = nuevoCorreoElectronico;
            existingUser.NormalizedEmail = nuevoCorreoElectronico.ToUpperInvariant();
            existingUser.UserName = nuevoCorreoElectronico;
            existingUser.NormalizedUserName = nuevoCorreoElectronico.ToUpperInvariant();
            existingUser.EmailConfirmed = false;

            var updateResult = await userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            var securityStampResult = await userManager.UpdateSecurityStampAsync(existingUser);
            if (!securityStampResult.Succeeded)
            {
                foreach (var error in securityStampResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            var updatedUser = await userManager.FindByIdAsync(existingUser.Id);
            if (updatedUser is null)
            {
                AddError("El usuario fue actualizado pero no pudo ser recuperado para enviar la validacion del nuevo correo.");
                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            emailDispatch = await emailConfirmationMessageService.CreateDispatchAsync(updatedUser);
            var emailMessage = emailConfirmationMessageService.BuildEmailMessage(updatedUser, emailDispatch);
            notificationOutboxService.QueueEmail(
                emailMessage,
                NotificationOutboxNotificationTypes.EmailConfirmation,
                updatedUser.Id,
                command.Request.Path,
                $"email-change-confirmation:{updatedUser.Id}:{emailDispatch.ValidationToken}");
            securityTraceabilityService.TrackUpdate(
                command,
                updatedUser.Id,
                SecurityTraceabilityEventTypes.CorreoElectronicoModificado,
                beforeState,
                UsuarioTraceabilityState.FromUser(updatedUser) with
                {
                    ActionContext = "EMAIL_CHANGE",
                    RequestPath = command.Request.Path
                });
            securityTraceabilityService.TrackCreate(
                command,
                updatedUser.Id,
                SecurityTraceabilityEventTypes.CorreoValidacionCambioCorreoProgramado,
                UsuarioTraceabilityState.FromUser(updatedUser) with
                {
                    ActionContext = "EMAIL_CHANGE",
                    RequestPath = command.Request.Path,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.EmailConfirmation,
                    ValidationToken = emailDispatch.ValidationToken
                });

            if (!string.IsNullOrWhiteSpace(currentUserService.SessionId))
            {
                await tokenService.RevokeSessionAsync(currentUserService.SessionId, EnumRefreshTokenRevocationReasons.Logout);
            }

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad del cambio de correo electronico.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(
                ex,
                "No fue posible modificar el correo electronico del usuario {UserId}.",
                command.IdUsuario);
            AddError("No fue posible modificar el correo electronico en este momento.");
            return CommandResponse;
        }

        authCookieService.ClearAuthCookies(command.Response);
        csrfService.EnsureTokenCookie(command.Response);

        CommandResponse.Data = new EmailConfirmationDispatchResponse(
            Email: nuevoCorreoElectronico,
            Message: "Correo electronico actualizado. Debes validar el nuevo correo para volver a ingresar a la plataforma.",
            ConfirmationUrl: emailDispatch?.ManualConfirmationUrl,
            ValidationToken: emailDispatch is null
                ? null
                : emailConfirmationMessageService.GetValidationTokenForResponse(emailDispatch));
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
