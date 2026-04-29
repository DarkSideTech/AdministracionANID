using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class NoRequiereValidacionAlSerAsignadoCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<NoRequiereValidacionAlSerAsignadoCommandHandler> logger) : RolFlagCommandHandler<NoRequiereValidacionAlSerAsignadoCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => false;
    protected override string OperationName => "desactivar validacion al asignar rol";
    protected override string AlreadyMessage => "El rol ya no requiere validacion al ser asignado.";
    protected override string SuccessMessage => "Validacion requerida al asignar rol desactivada correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.RequiereValidacionDeAsignacion;
    protected override void SetCurrentValue(Rol role, bool value) => role.RequiereValidacionDeAsignacion = value;
}
