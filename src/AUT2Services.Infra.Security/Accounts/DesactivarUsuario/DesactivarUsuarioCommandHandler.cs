using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.DesactivarUsuario;

public class DesactivarUsuarioCommandHandler : CommandHandler,
    IRequestHandler<DesactivarUsuarioCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly ICsrfService csrfService;
    private readonly ICurrentUserService currentUserService;
    private readonly ISessionValidationService sessionValidationService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ILogger<DesactivarUsuarioCommandHandler> logger;

    public DesactivarUsuarioCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        ISecurityTraceabilityService securityTraceabilityService,
        ILogger<DesactivarUsuarioCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.csrfService = csrfService;
        this.currentUserService = currentUserService;
        this.sessionValidationService = sessionValidationService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(DesactivarUsuarioCommand command, CancellationToken cancellationToken)
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

        var existingUser = await userManager.FindByIdAsync(command.IdUsuario!);
        if (existingUser is null)
        {
            AddError("El usuario no existe.");
            return CommandResponse;
        }

        if (existingUser.UsuarioBase == true)
        {
            AddError("El usuario base no puede ser modificado.");
            return CommandResponse;
        }

        if (existingUser.Activo != true)
        {
            CommandResponse.Data = new ActivarDesactivarUsuarioResponse(
                existingUser.Id,
                "El usuario ya se encuentra desactivado.");
            CommandResponse.Result = true;
            return CommandResponse;
        }

        var beforeState = UsuarioTraceabilityState.FromUser(existingUser);
        existingUser.Activo = false;

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var updateResult = await userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            securityTraceabilityService.TrackUpdate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.UsuarioDesactivado,
                beforeState,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "USER_DEACTIVATE",
                    RequestPath = command.Request.Path
                },
                includeSnapshot: true);

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad de la desactivacion del usuario.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible desactivar el usuario {UserId}.", command.IdUsuario);
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            AddError("No fue posible desactivar el usuario en este momento.");
            return CommandResponse;
        }

        CommandResponse.Data = new ActivarDesactivarUsuarioResponse(
            existingUser.Id,
            "El usuario fue desactivado correctamente.");
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
