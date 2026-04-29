using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class DesactivarRolCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<DesactivarRolCommandHandler> logger) : RolFlagCommandHandler<DesactivarRolCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => false;
    protected override string OperationName => "desactivar rol";
    protected override string AlreadyMessage => "El rol ya se encuentra desactivado.";
    protected override string SuccessMessage => "Rol desactivado correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.Activo;
    protected override void SetCurrentValue(Rol role, bool value) => role.Activo = value;
}
