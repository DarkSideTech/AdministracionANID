using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
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

namespace AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;

public class ConfirmaRecuperacionClaveCommandHandler : CommandHandler,
    IRequestHandler<ConfirmaRecuperacionClaveCommand, CommandResponse>
{
    private const string GenericConfirmationError = "No fue posible confirmar la recuperacion de clave. Verifica el codigo o solicita uno nuevo.";

    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly ICsrfService csrfService;
    private readonly IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly PasswordChangeOptions passwordChangeOptions;
    private readonly IClock clock;
    private readonly ILogger<ConfirmaRecuperacionClaveCommandHandler> logger;

    public ConfirmaRecuperacionClaveCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        ICsrfService csrfService,
        IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService,
        ISecurityTraceabilityService securityTraceabilityService,
        IOptions<PasswordChangeOptions> passwordChangeOptions,
        IClock clock,
        ILogger<ConfirmaRecuperacionClaveCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.csrfService = csrfService;
        this.passwordChangeChallengeMessageService = passwordChangeChallengeMessageService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.passwordChangeOptions = passwordChangeOptions.Value;
        this.clock = clock;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ConfirmaRecuperacionClaveCommand command, CancellationToken cancellationToken)
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
        var existingUser = await FindEligibleUserAsync(email);
        if (existingUser is null)
        {
            AddError(GenericConfirmationError);
            return CommandResponse;
        }

        var now = clock.UtcNow;
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
            AddError(GenericConfirmationError);
            return CommandResponse;
        }

        if (!passwordChangeChallengeMessageService.IsCodeMatch(existingUser.Id, command.CodigoValidacion!.Trim(), activeChallenge.CodeHash))
        {
            return await RegisterInvalidCodeAsync(command, existingUser, activeChallenge, now);
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var removePasswordResult = await userManager.RemovePasswordAsync(existingUser);
            if (!removePasswordResult.Succeeded)
            {
                foreach (var error in removePasswordResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            var addPasswordResult = await userManager.AddPasswordAsync(existingUser, command.NuevaClave!);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            var securityStampResult = await userManager.UpdateSecurityStampAsync(existingUser);
            if (!securityStampResult.Succeeded)
            {
                foreach (var error in securityStampResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            activeChallenge.ConsumedAtUtc = now;
            dbContext.PasswordChangeChallenges.Update(activeChallenge);
            await RevokeActiveRefreshTokensAsync(existingUser.Id, now, cancellationToken);

            securityTraceabilityService.TrackCreate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.ValidacionRecuperacionClaveRespondida,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "PASSWORD_RECOVERY_CONFIRMATION",
                    RequestPath = command.Request.Path,
                    ChallengeId = activeChallenge.Id,
                    RespondedAtUtc = now,
                    Result = "SUCCESS"
                });
            securityTraceabilityService.TrackCreate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.ClaveAccesoRecuperada,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "PASSWORD_RECOVERY",
                    RequestPath = command.Request.Path,
                    ChallengeId = activeChallenge.Id,
                    RespondedAtUtc = now,
                    Result = "SUCCESS"
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad de la recuperacion de clave.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(ex, "No fue posible confirmar la recuperacion de clave para el correo {Email}.", email);
            AddError("No fue posible recuperar la clave en este momento.");
            return CommandResponse;
        }

        csrfService.EnsureTokenCookie(command.Response);

        CommandResponse.Data = new PasswordChangeResultResponse(
            Message: "Clave recuperada correctamente. Debes iniciar sesion con tu nueva clave.");
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

    private async Task<CommandResponse> RegisterInvalidCodeAsync(
        ConfirmaRecuperacionClaveCommand command,
        Usuario existingUser,
        PasswordChangeChallenge activeChallenge,
        DateTimeOffset now)
    {
        activeChallenge.FailedAttempts += 1;
        if (activeChallenge.FailedAttempts >= passwordChangeOptions.MaxFailedAttempts)
        {
            activeChallenge.CancelledAtUtc = now;
        }

        dbContext.PasswordChangeChallenges.Update(activeChallenge);
        securityTraceabilityService.TrackCreate(
            command,
            existingUser.Id,
            SecurityTraceabilityEventTypes.ValidacionRecuperacionClaveRespondida,
            UsuarioTraceabilityState.FromUser(existingUser) with
            {
                ActionContext = "PASSWORD_RECOVERY_CONFIRMATION",
                RequestPath = command.Request.Path,
                ChallengeId = activeChallenge.Id,
                RespondedAtUtc = now,
                Result = activeChallenge.CancelledAtUtc is null ? "INVALID_CODE" : "INVALID_CODE_CHALLENGE_CANCELLED"
            });

        if (!await dbContext.Commit())
        {
            AddError("No fue posible persistir la trazabilidad de la validacion de recuperacion de clave.");
            return CommandResponse;
        }

        AddError(activeChallenge.CancelledAtUtc is null
            ? GenericConfirmationError
            : "La solicitud de recuperacion fue invalidada. Debes solicitar un nuevo codigo.");
        return CommandResponse;
    }

    private async Task RevokeActiveRefreshTokensAsync(string userId, DateTimeOffset now, CancellationToken cancellationToken)
    {
        var activeRefreshTokens = await dbContext.RefreshTokens
            .AsTracking()
            .Where(token => token.UserId == userId
                && token.RevokedAtUtc == null
                && (token.ExpiresAtUtc == null || token.ExpiresAtUtc > now))
            .ToListAsync(cancellationToken);

        foreach (var token in activeRefreshTokens)
        {
            token.RevokedAtUtc = now;
            token.RevocationReason = EnumRefreshTokenRevocationReasons.PasswordRecovery;
            dbContext.RefreshTokens.Update(token);
        }
    }
}
