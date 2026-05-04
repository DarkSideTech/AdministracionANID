using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public abstract class RolCommandHandlerBase : CommandHandler
{
    protected readonly RoleManager<Rol> RoleManager;
    protected readonly ICurrentUserService CurrentUserService;
    private readonly ICsrfService csrfService;
    private readonly ISessionValidationService sessionValidationService;

    protected RolCommandHandlerBase(
        RoleManager<Rol> roleManager,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService)
    {
        RoleManager = roleManager;
        this.csrfService = csrfService;
        CurrentUserService = currentUserService;
        this.sessionValidationService = sessionValidationService;
    }

    protected async Task<bool> ValidateAuthenticatedMutationAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        if (!csrfService.IsRequestValid(request))
        {
            AddError("Invalid CSRF token.");
            return false;
        }

        return await ValidateAuthenticatedSessionAsync(cancellationToken);
    }

    protected async Task<bool> ValidateAuthenticatedSessionAsync(CancellationToken cancellationToken)
    {
        if (!CurrentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(CurrentUserService.UserId))
        {
            AddError("Usuario no autorizado.");
            return false;
        }

        var principal = CurrentUserService.GetClaimsPrincipal(cancellationToken);
        var securityStamp = principal?.FindFirst(EnumTokenValidationClaims.SecurityStamp)?.Value;
        var isSessionValid = await sessionValidationService.IsSessionValidAsync(
            CurrentUserService.UserId,
            CurrentUserService.SessionId,
            securityStamp,
            cancellationToken);

        if (!isSessionValid)
        {
            AddError("La sesion del usuario no es valida.");
            return false;
        }

        return true;
    }

    protected async Task<Rol?> FindMutableRoleAsync(string idRol)
    {
        var existingRole = await RoleManager.FindByIdAsync(idRol);
        if (existingRole is null)
        {
            AddError("El rol no existe.");
            return null;
        }

        if (existingRole.RolBase == true)
        {
            AddError("El rol base no puede ser modificado.");
            return null;
        }

        return existingRole;
    }

    protected async Task<bool> PersistRoleAsync(Rol role)
    {
        var updateResult = await RoleManager.UpdateAsync(role);
        if (updateResult.Succeeded)
        {
            return true;
        }

        foreach (var error in updateResult.Errors)
        {
            AddError($"{error.Code} - {error.Description}");
        }

        return false;
    }
}
