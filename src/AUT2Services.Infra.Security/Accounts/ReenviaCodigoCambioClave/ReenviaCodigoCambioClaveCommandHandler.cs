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

namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;

public class ReenviaCodigoCambioClaveCommandHandler : CommandHandler,
    IRequestHandler<ReenviaCodigoCambioClaveCommand, CommandResponse>
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
    private readonly ILogger<ReenviaCodigoCambioClaveCommandHandler> logger;

    public ReenviaCodigoCambioClaveCommandHandler(
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
        ILogger<ReenviaCodigoCambioClaveCommandHandler> logger)
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

    public async Task<CommandResponse> Handle(ReenviaCodigoCambioClaveCommand command, CancellationToken cancellationToken)
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

        if (!existingUser.EmailConfirmed || existingUser.Activo != true || string.IsNullOrWhiteSpace(existingUser.Email))
        {
            AddError("El usuario no se encuentra habilitado para reenviar el codigo de cambio de clave.");
            return CommandResponse;
        }

        if (!string.Equals(existingUser.EstadoDeUsuario, EnumEstadoDeUsuario.REGISTRADO, StringComparison.OrdinalIgnoreCase))
        {
            AddError("El usuario no se encuentra en estado REGISTRADO.");
            return CommandResponse;
        }

        if (!await userManager.HasPasswordAsync(existingUser))
        {
            AddError("El usuario autenticado no cuenta con una clave local modificable.");
            return CommandResponse;
        }

        var now = clock.UtcNow;
        var activeChallenge = await dbContext.PasswordChangeChallenges
            .AsTracking()
            .Where(item => item.UserId == existingUser.Id
                && item.ChallengePurpose == PasswordChangeChallengePurposes.PasswordChange
                && item.ConsumedAtUtc == null
                && item.CancelledAtUtc == null)
            .OrderByDescending(item => item.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (activeChallenge is null || activeChallenge.ExpiresAtUtc <= now)
        {
            AddError("No existe una solicitud vigente de cambio de clave. Solicita un nuevo codigo de validacion.");
            return CommandResponse;
        }

        var canResendAtUtc = activeChallenge.LastSentAtUtc.AddSeconds(passwordChangeOptions.ResendCooldownSeconds);
        if (canResendAtUtc > now)
        {
            AddError($"Aun no es posible reenviar un nuevo codigo. Intenta nuevamente despues de las {canResendAtUtc:HH:mm:ss} UTC.");
            return CommandResponse;
        }

        var expiresAtUtc = now.AddMinutes(passwordChangeOptions.CodeLifetimeMinutes);
        var dispatch = passwordChangeChallengeMessageService.CreateDispatch(existingUser, expiresAtUtc);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            activeChallenge.CancelledAtUtc = now;
            dbContext.PasswordChangeChallenges.Update(activeChallenge);

            dbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
            {
                Id = Guid.NewGuid(),
                UserId = existingUser.Id,
                ChallengePurpose = PasswordChangeChallengePurposes.PasswordChange,
                CodeHash = dispatch.CodeHash,
                RequestPath = command.Request.Path,
                FailedAttempts = 0,
                ResendCount = activeChallenge.ResendCount + 1,
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
                    ActionContext = "RESEND",
                    RequestPath = command.Request.Path,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.PasswordChangeVerificationCode,
                    ValidationCode = dispatch.Code,
                    ExpiresAtUtc = expiresAtUtc
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad del reenvio del codigo de cambio de clave.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(ex, "No fue posible reenviar el codigo de cambio de clave para el usuario {UserId}.", command.IdUsuario);
            AddError("No fue posible reenviar el codigo de validacion.");
            return CommandResponse;
        }

        CommandResponse.Data = new PasswordChangeChallengeDispatchResponse(
            Email: existingUser.Email ?? string.Empty,
            Message: "Se envio un nuevo codigo de validacion al correo electronico registrado.",
            ExpiresAtUtc: expiresAtUtc,
            CanResendAtUtc: now.AddSeconds(passwordChangeOptions.ResendCooldownSeconds));
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
