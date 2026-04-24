using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;

public class SolicitaCambioClaveCommandHandler : CommandHandler,
    IRequestHandler<SolicitaCambioClaveCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly INotificationOutboxService notificationOutboxService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ICsrfService csrfService;
    private readonly ICurrentUserService currentUserService;
    private readonly ISessionValidationService sessionValidationService;
    private readonly IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService;
    private readonly PasswordChangeOptions passwordChangeOptions;
    private readonly IClock clock;
    private readonly ILogger<SolicitaCambioClaveCommandHandler> logger;

    public SolicitaCambioClaveCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        INotificationOutboxService notificationOutboxService,
        ISecurityTraceabilityService securityTraceabilityService,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService,
        IOptions<PasswordChangeOptions> passwordChangeOptions,
        IClock clock,
        ILogger<SolicitaCambioClaveCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.notificationOutboxService = notificationOutboxService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.csrfService = csrfService;
        this.currentUserService = currentUserService;
        this.sessionValidationService = sessionValidationService;
        this.passwordChangeChallengeMessageService = passwordChangeChallengeMessageService;
        this.passwordChangeOptions = passwordChangeOptions.Value;
        this.clock = clock;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(SolicitaCambioClaveCommand command, CancellationToken cancellationToken)
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

        var existingUser = await ValidateEligibleUserAsync(command.IdUsuario!);
        if (existingUser is null)
        {
            return CommandResponse;
        }

        if (!await userManager.CheckPasswordAsync(existingUser, command.ClaveActual!))
        {
            AddError("La clave anterior no corresponde a la clave actual.");
            return CommandResponse;
        }

        var now = clock.UtcNow;
        var expiresAtUtc = now.AddMinutes(passwordChangeOptions.CodeLifetimeMinutes);
        var canResendAtUtc = now.AddSeconds(passwordChangeOptions.ResendCooldownSeconds);
        var dispatch = passwordChangeChallengeMessageService.CreateDispatch(existingUser, expiresAtUtc);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await CancelActiveChallengesAsync(existingUser.Id, now, cancellationToken);

            dbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
            {
                Id = Guid.NewGuid(),
                UserId = existingUser.Id,
                ChallengePurpose = PasswordChangeChallengePurposes.PasswordChange,
                CodeHash = dispatch.CodeHash,
                RequestPath = command.Request.Path,
                FailedAttempts = 0,
                ResendCount = 0,
                CreatedAtUtc = now,
                LastSentAtUtc = now,
                ExpiresAtUtc = expiresAtUtc
            });

            notificationOutboxService.QueueEmail(
                dispatch.EmailMessage,
                NotificationOutboxNotificationTypes.PasswordChangeVerificationCode,
                existingUser.Id,
                command.Request.Path,
                $"password-change:{existingUser.Id}:{dispatch.CodeHash}");
            securityTraceabilityService.TrackCreate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.CodigoValidacionCambioClaveProgramado,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "INITIAL",
                    RequestPath = command.Request.Path,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.PasswordChangeVerificationCode,
                    ValidationCode = dispatch.Code,
                    ExpiresAtUtc = expiresAtUtc
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad de la solicitud de cambio de clave.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(ex, "No fue posible solicitar el cambio de clave para el usuario {UserId}.", command.IdUsuario);
            AddError("No fue posible generar el codigo de validacion para el cambio de clave.");
            return CommandResponse;
        }

        CommandResponse.Data = new PasswordChangeChallengeDispatchResponse(
            Email: existingUser.Email ?? string.Empty,
            Message: "Se envio un codigo de validacion al correo electronico registrado.",
            ExpiresAtUtc: expiresAtUtc,
            CanResendAtUtc: canResendAtUtc);
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private async Task<Usuario?> ValidateEligibleUserAsync(string userId)
    {
        var existingUser = await userManager.FindByIdAsync(userId);
        if (existingUser is null)
        {
            AddError("El usuario no existe.");
            return null;
        }

        if (!existingUser.EmailConfirmed)
        {
            AddError("El usuario debe tener un correo electronico validado para cambiar la clave.");
            return null;
        }

        if (existingUser.Activo != true)
        {
            AddError("El usuario no se encuentra activo.");
            return null;
        }

        if (!string.Equals(existingUser.EstadoDeUsuario, EnumEstadoDeUsuario.REGISTRADO, StringComparison.OrdinalIgnoreCase))
        {
            AddError("El usuario no se encuentra en estado REGISTRADO.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(existingUser.Email))
        {
            AddError("El usuario no tiene un correo electronico valido para enviar el codigo de validacion.");
            return null;
        }

        if (!await userManager.HasPasswordAsync(existingUser))
        {
            AddError("El usuario autenticado no cuenta con una clave local modificable.");
            return null;
        }

        return existingUser;
    }

    private async Task CancelActiveChallengesAsync(string userId, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var activeChallenges = await dbContext.PasswordChangeChallenges
            .AsTracking()
            .Where(item => item.UserId == userId
                && item.ChallengePurpose == PasswordChangeChallengePurposes.PasswordChange
                && item.ConsumedAtUtc == null
                && item.CancelledAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var activeChallenge in activeChallenges)
        {
            activeChallenge.CancelledAtUtc = now;
            dbContext.PasswordChangeChallenges.Update(activeChallenge);
        }
    }
}
