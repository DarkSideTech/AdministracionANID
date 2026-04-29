using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class DesactivaValidacionDeAsignacionDeRolesCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<DesactivaValidacionDeAsignacionDeRolesCommandHandler> logger) : RolFlagCommandHandler<DesactivaValidacionDeAsignacionDeRolesCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => false;
    protected override string OperationName => "desactivar validacion de asignacion de roles";
    protected override string AlreadyMessage => "El rol ya no valida asignacion de roles.";
    protected override string SuccessMessage => "Validacion de asignacion de roles desactivada correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.ValidaAsignacionDeRoles;
    protected override void SetCurrentValue(Rol role, bool value) => role.ValidaAsignacionDeRoles = value;
}
