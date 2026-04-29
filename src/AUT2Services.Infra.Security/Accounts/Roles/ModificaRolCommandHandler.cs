using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ModificaRolCommandHandler : RolCommandHandlerBase,
    IRequestHandler<ModificaRolCommand, CommandResponse>
{
    private readonly ILogger<ModificaRolCommandHandler> logger;

    public ModificaRolCommandHandler(
        RoleManager<Rol> roleManager,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        ILogger<ModificaRolCommandHandler> logger)
        : base(roleManager, csrfService, currentUserService, sessionValidationService)
    {
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ModificaRolCommand command, CancellationToken cancellationToken)
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

        try
        {
            existingRole.Descripcion = command.Descripcion?.Trim() ?? string.Empty;
            existingRole.ValidaEnrrolamiento = command.ValidaEnrrolamiento == true;
            existingRole.ValidaAsignacionDeRoles = command.ValidaAsignacionDeRoles == true;

            if (!await PersistRoleAsync(existingRole))
            {
                return CommandResponse;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible modificar el rol {RoleId}.", command.IdRol);
            AddError("No fue posible modificar el rol en este momento.");
            return CommandResponse;
        }

        CommandResponse.Data = new RolCommandResponse(existingRole.Id, "Rol modificado correctamente.");
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
