using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ActivarRolCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<ActivarRolCommandHandler> logger) : RolFlagCommandHandler<ActivarRolCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => true;
    protected override string OperationName => "activar rol";
    protected override string AlreadyMessage => "El rol ya se encuentra activo.";
    protected override string SuccessMessage => "Rol activado correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.Activo;
    protected override void SetCurrentValue(Rol role, bool value) => role.Activo = value;
}
