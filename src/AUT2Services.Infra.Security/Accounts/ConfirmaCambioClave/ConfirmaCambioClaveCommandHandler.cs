using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;

public class ConfirmaCambioClaveCommandHandler : CommandHandler,
    IRequestHandler<ConfirmaCambioClaveCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly ICsrfService csrfService;
    private readonly ICurrentUserService currentUserService;
    private readonly ISessionValidationService sessionValidationService;
    private readonly IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService;
    private readonly ITokenService tokenService;
    private readonly IAuthCookieService authCookieService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly PasswordChangeOptions passwordChangeOptions;
    private readonly IClock clock;
    private readonly ILogger<ConfirmaCambioClaveCommandHandler> logger;

    public ConfirmaCambioClaveCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        IPasswordChangeChallengeMessageService passwordChangeChallengeMessageService,
        ITokenService tokenService,
        IAuthCookieService authCookieService,
        ISecurityTraceabilityService securityTraceabilityService,
        IOptions<PasswordChangeOptions> passwordChangeOptions,
        IClock clock,
        ILogger<ConfirmaCambioClaveCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.csrfService = csrfService;
        this.currentUserService = currentUserService;
        this.sessionValidationService = sessionValidationService;
        this.passwordChangeChallengeMessageService = passwordChangeChallengeMessageService;
        this.tokenService = tokenService;
        this.authCookieService = authCookieService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.passwordChangeOptions = passwordChangeOptions.Value;
        this.clock = clock;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ConfirmaCambioClaveCommand command, CancellationToken cancellationToken)
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
            AddError("El usuario no se encuentra habilitado para cambiar la clave.");
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
                && item.ConsumedAtUtc == null
                && item.CancelledAtUtc == null)
            .OrderByDescending(item => item.CreatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (activeChallenge is null || activeChallenge.ExpiresAtUtc <= now)
        {
            AddError("No existe una solicitud vigente de cambio de clave o el codigo ya expiró.");
            return CommandResponse;
        }

        if (!passwordChangeChallengeMessageService.IsCodeMatch(existingUser.Id, command.CodigoValidacion!.Trim(), activeChallenge.CodeHash))
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
                SecurityTraceabilityEventTypes.ValidacionCambioClaveRespondida,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "PASSWORD_CHANGE_CONFIRMATION",
                    RequestPath = command.Request.Path,
                    ChallengeId = activeChallenge.Id,
                    RespondedAtUtc = now,
                    Result = activeChallenge.CancelledAtUtc is null ? "INVALID_CODE" : "INVALID_CODE_CHALLENGE_CANCELLED"
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad de la validacion del cambio de clave.");
                return CommandResponse;
            }

            AddError(activeChallenge.CancelledAtUtc is null
                ? "El codigo de validacion ingresado no es correcto."
                : "El codigo de validacion ingresado no es correcto y la solicitud fue invalidada. Debes solicitar un nuevo codigo.");
            return CommandResponse;
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var changePasswordResult = await userManager.ChangePasswordAsync(existingUser, command.ClaveActual!, command.NuevaClave!);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await transaction.RollbackAsync(cancellationToken);
                return CommandResponse;
            }

            activeChallenge.ConsumedAtUtc = now;
            dbContext.PasswordChangeChallenges.Update(activeChallenge);
            securityTraceabilityService.TrackCreate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.ValidacionCambioClaveRespondida,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "PASSWORD_CHANGE_CONFIRMATION",
                    RequestPath = command.Request.Path,
                    ChallengeId = activeChallenge.Id,
                    RespondedAtUtc = now,
                    Result = "SUCCESS"
                });
            securityTraceabilityService.TrackCreate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.ClaveAccesoModificada,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "PASSWORD_CHANGE",
                    RequestPath = command.Request.Path,
                    ChallengeId = activeChallenge.Id,
                    RespondedAtUtc = now,
                    Result = "SUCCESS"
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad del cambio de clave.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            logger.LogError(ex, "No fue posible confirmar el cambio de clave del usuario {UserId}.", command.IdUsuario);
            AddError("No fue posible cambiar la clave en este momento.");
            return CommandResponse;
        }

        if (!string.IsNullOrWhiteSpace(currentUserService.SessionId))
        {
            try
            {
                await tokenService.RevokeSessionAsync(currentUserService.SessionId, EnumRefreshTokenRevocationReasons.Logout);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "La clave fue cambiada, pero no fue posible revocar la sesion {SessionId}.", currentUserService.SessionId);
            }
        }

        authCookieService.ClearAuthCookies(command.Response);
        csrfService.EnsureTokenCookie(command.Response);

        CommandResponse.Data = new PasswordChangeResultResponse(
            Message: "Clave modificada correctamente. Debes iniciar sesion nuevamente.");
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
