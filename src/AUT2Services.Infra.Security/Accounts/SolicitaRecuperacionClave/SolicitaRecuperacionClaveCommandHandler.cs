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

namespace AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;

public class SolicitaRecuperacionClaveCommandHandler : CommandHandler,
    IRequestHandler<SolicitaRecuperacionClaveCommand, CommandResponse>
{
    private const string GenericMessage = "Si el correo ingresado corresponde a una cuenta vigente, enviaremos un codigo de validacion para recuperar la clave.";

    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly INotificationOutboxService notificationOutboxService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ICsrfService csrfService;
    private readonly IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService;
    private readonly PasswordChangeOptions passwordChangeOptions;
    private readonly IClock clock;
    private readonly ILogger<SolicitaRecuperacionClaveCommandHandler> logger;

    public SolicitaRecuperacionClaveCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        INotificationOutboxService notificationOutboxService,
        ISecurityTraceabilityService securityTraceabilityService,
        ICsrfService csrfService,
        IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService,
        IOptions<PasswordChangeOptions> passwordChangeOptions,
        IClock clock,
        ILogger<SolicitaRecuperacionClaveCommandHandler> logger)
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

    public async Task<CommandResponse> Handle(SolicitaRecuperacionClaveCommand command, CancellationToken cancellationToken)
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

        var expiresAtUtc = now.AddMinutes(passwordChangeOptions.CodeLifetimeMinutes);
        var dispatch = passwordChangeChallengeMessageService.CreateRecoveryDispatch(existingUser, expiresAtUtc);
        var challengeId = Guid.NewGuid();

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await CancelActiveRecoveryChallengesAsync(existingUser.Id, now, cancellationToken);

            dbContext.PasswordChangeChallenges.Add(new PasswordChangeChallenge
            {
                Id = challengeId,
                UserId = existingUser.Id,
                ChallengePurpose = PasswordChangeChallengePurposes.PasswordRecovery,
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
                    ActionContext = "INITIAL",
                    RequestPath = command.Request.Path,
                    ChallengeId = challengeId,
                    NotificationChannel = NotificationOutboxChannels.Email,
                    NotificationType = NotificationOutboxNotificationTypes.PasswordRecoveryVerificationCode,
                    ExpiresAtUtc = expiresAtUtc
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la solicitud de recuperacion de clave.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(ex, "No fue posible solicitar recuperacion de clave para el correo {Email}.", email);
            AddError("No fue posible generar el codigo de validacion para recuperar la clave.");
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

    private async Task CancelActiveRecoveryChallengesAsync(string userId, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var activeChallenges = await dbContext.PasswordChangeChallenges
            .AsTracking()
            .Where(item => item.UserId == userId
                && item.ChallengePurpose == PasswordChangeChallengePurposes.PasswordRecovery
                && item.ConsumedAtUtc == null
                && item.CancelledAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var activeChallenge in activeChallenges)
        {
            activeChallenge.CancelledAtUtc = now;
            dbContext.PasswordChangeChallenges.Update(activeChallenge);
        }
    }

    private CommandResponse SucceedWithNeutralResponse(string email, DateTimeOffset now)
    {
        CommandResponse.Data = new PasswordChangeChallengeDispatchResponse(
            Email: email,
            Message: GenericMessage,
            ExpiresAtUtc: now.AddMinutes(passwordChangeOptions.CodeLifetimeMinutes),
            CanResendAtUtc: now.AddSeconds(passwordChangeOptions.ResendCooldownSeconds));
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
