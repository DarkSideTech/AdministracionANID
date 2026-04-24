using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoRecuperacionClave;

public class ReenviaCodigoRecuperacionClaveCommandHandler : CommandHandler,
    IRequestHandler<ReenviaCodigoRecuperacionClaveCommand, CommandResponse>
{
    private const string GenericMessage = "Si el correo ingresado corresponde a una cuenta vigente con una solicitud activa, enviaremos un nuevo codigo de validacion.";

    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly INotificationOutboxService notificationOutboxService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ICsrfService csrfService;
    private readonly IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService;
    private readonly PasswordChangeOptions passwordChangeOptions;
    private readonly IClock clock;
    private readonly ILogger<ReenviaCodigoRecuperacionClaveCommandHandler> logger;

    public ReenviaCodigoRecuperacionClaveCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        INotificationOutboxService notificationOutboxService,
        ISecurityTraceabilityService securityTraceabilityService,
        ICsrfService csrfService,
        IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService,
        IOptions<PasswordChangeOptions> passwordChangeOptions,
        IClock clock,
        ILogger<ReenviaCodigoRecuperacionClaveCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.notificationOutboxService = notificationOutboxService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.csrfService = csrfService;
        this.passwordChangeChallengeMessageService = passwordChangeChallengeMessageService;
        this.passwordChangeOptions = passwordChangeOptions.Value;
        this.clock = clock;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ReenviaCodigoRecuperacionClaveCommand command, CancellationToken cancellationToken)
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

        var email = command.CorreoElectronico!.Trim();
        var now = clock.UtcNow;
        var existingUser = await FindEligibleUserAsync(email);

        if (existingUser is null)
        {
            return SucceedWithNeutralResponse(email, now);
        }

        var activeChallenge = await dbContext.PasswordChangeChallenges
            .AsTracking()
            .Where(item => item.UserId == existingUser.Id
                && item.ChallengePurpose == PasswordChangeChallengePurposes.PasswordRecovery
                && item.ConsumedAtUtc == null
                && item.CancelledAtUtc == null)
            .OrderByDescending(item => item.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (activeChallenge is null || activeChallenge.ExpiresAtUtc <= now)
        {
            return SucceedWithNeutralResponse(email, now);
        }

        var canResendAtUtc = activeChallenge.LastSentAtUtc.AddSeconds(passwordChangeOptions.ResendCooldownSeconds);
        if (canResendAtUtc > now)
        {
            return SucceedWithNeutralResponse(email, now, canResendAtUtc);
        }

        var expiresAtUtc = now.AddMinutes(passwordChangeOptions.CodeLifetimeMinutes);
        var dispatch = passwordChangeChallengeMessageService.CreateRecoveryDispatch(existingUser, expiresAtUtc);
        var challengeId = Guid.NewGuid();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            activeChallenge.CancelledAtUtc = now;
            dbContext.PasswordChangeChallenges.Update(activeChallenge);

            dbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
            {
                Id = challengeId,
                UserId = existingUser.Id,
                ChallengePurpose = PasswordChangeChallengePurposes.PasswordRecovery,
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
                NotificationOutboxNotificationTypes.PasswordRecoveryVerificationCode,
                existingUser.Id,
                command.Request.Path,
                $"password-recovery:{existingUser.Id}:{dispatch.CodeHash}");
            securityTraceabilityService.TrackCreate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.CodigoValidacionRecuperacionClaveProgramado,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "RESEND",
                    RequestPath = command.Request.Path,
                    ChallengeId = challengeId,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.PasswordRecoveryVerificationCode,
                    ExpiresAtUtc = expiresAtUtc
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir el reenvio del codigo de recuperacion de clave.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(ex, "No fue posible reenviar recuperacion de clave para el correo {Email}.", email);
            AddError("No fue posible reenviar el codigo de validacion.");
            return CommandResponse;
        }

        CommandResponse.Data = new PasswordChangeChallengeDispatchResponse(
            Email: email,
            Message: GenericMessage,
            ExpiresAtUtc: expiresAtUtc,
            CanResendAtUtc: now.AddSeconds(passwordChangeOptions.ResendCooldownSeconds));
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private async Task<Usuario?> FindEligibleUserAsync(string email)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is null)
        {
            return null;
        }

        if (!existingUser.EmailConfirmed || existingUser.Activo != true || string.IsNullOrWhiteSpace(existingUser.Email))
        {
            return null;
        }

        if (!string.Equals(existingUser.EstadoDeUsuario, EnumEstadoDeUsuario.REGISTRADO, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return await userManager.HasPasswordAsync(existingUser) ? existingUser : null;
    }

    private CommandResponse SucceedWithNeutralResponse(string email, DateTimeOffset now, DateTimeOffset? canResendAtUtc = null)
    {
        CommandResponse.Data = new PasswordChangeChallengeDispatchResponse(
            Email: email,
            Message: GenericMessage,
            ExpiresAtUtc: now.AddMinutes(passwordChangeOptions.CodeLifetimeMinutes),
            CanResendAtUtc: canResendAtUtc ?? now.AddSeconds(passwordChangeOptions.ResendCooldownSeconds));
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
