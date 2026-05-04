using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class BuscarRolesCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService) : RolCommandHandlerBase(roleManager, csrfService, currentUserService, sessionValidationService),
    IRequestHandler<BuscarRolesCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(BuscarRolesCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!await ValidateAuthenticatedSessionAsync(cancellationToken))
        {
            return CommandResponse;
        }

        var estado = NormalizeState(command.Estado);
        var query = RoleManager.Roles.AsNoTracking();

        query = estado switch
        {
            "ACTIVOS" => query.Where(rol => rol.Activo == true),
            "INACTIVOS" => query.Where(rol => rol.Activo != true),
            _ => query
        };

        if (!UsuarioActualEsAdministrador())
        {
            query = query.Where(rol => rol.NormalizedName != EnumRolesBase.ADMINISTRADOR);
        }

        var roles = await query
            .OrderBy(rol => rol.NormalizedName ?? rol.Name)
            .ThenBy(rol => rol.Id)
            .Select(rol => new RolListaItemResponse(
                rol.Id,
                rol.NormalizedName ?? rol.Name))
            .ToArrayAsync(cancellationToken);

        CommandResponse.Data = new BuscarRolesResponse(roles);
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private bool UsuarioActualEsAdministrador()
    {
        var principal = CurrentUserService.GetClaimsPrincipal();
        if (principal is null)
        {
            return false;
        }

        var roles = principal.Claims
            .Where(claim =>
                claim.Type == ClaimTypes.Role ||
                claim.Type == EnumProcesosBase.ADMINISTRACION ||
                claim.Type == $"{EnumProcesosBase.ADMINISTRACION}{EnumPartialBusinessClaimTypes._ROL}")
            .Select(claim => claim.Value);

        return roles.Any(role => string.Equals(role, EnumRolesBase.ADMINISTRADOR, StringComparison.OrdinalIgnoreCase));
    }

    private static string NormalizeState(string? value)
    {
        return (value ?? "ACTIVOS").Trim().ToUpperInvariant();
    }
}
