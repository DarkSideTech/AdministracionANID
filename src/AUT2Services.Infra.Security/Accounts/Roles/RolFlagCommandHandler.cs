using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public abstract class RolFlagCommandHandler<TCommand> : RolCommandHandlerBase, IRequestHandler<TCommand, CommandResponse>
    where TCommand : RolIdCommand
{
    private readonly ILogger logger;

    protected RolFlagCommandHandler(
        RoleManager<Rol> roleManager,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        ILogger logger)
        : base(roleManager, csrfService, currentUserService, sessionValidationService)
    {
        this.logger = logger;
    }

    protected abstract bool DesiredValue { get; }
    protected abstract string OperationName { get; }
    protected abstract string AlreadyMessage { get; }
    protected abstract string SuccessMessage { get; }
    protected abstract bool? GetCurrentValue(Rol role);
    protected abstract void SetCurrentValue(Rol role, bool value);

    public async Task<CommandResponse> Handle(TCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!await ValidateAuthenticatedMutationAsync(command.Request, cancellationToken))
        {
            return CommandResponse;
        }

        var existingRole = await FindMutableRoleAsync(command.IdRol!.Trim());
        if (existingRole is null)
        {
            return CommandResponse;
        }

        if (GetCurrentValue(existingRole) == DesiredValue)
        {
            CommandResponse.Data = new RolCommandResponse(existingRole.Id, AlreadyMessage);
            CommandResponse.Result = true;
            return CommandResponse;
        }

        try
        {
            SetCurrentValue(existingRole, DesiredValue);
            if (!await PersistRoleAsync(existingRole))
            {
                return CommandResponse;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible ejecutar {OperationName} para el rol {RoleId}.", OperationName, command.IdRol);
            AddError($"No fue posible ejecutar {OperationName} para el rol en este momento.");
            return CommandResponse;
        }

        CommandResponse.Data = new RolCommandResponse(existingRole.Id, SuccessMessage);
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
