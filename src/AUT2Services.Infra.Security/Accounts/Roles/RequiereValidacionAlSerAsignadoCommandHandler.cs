using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class RequiereValidacionAlSerAsignadoCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<RequiereValidacionAlSerAsignadoCommandHandler> logger) : RolFlagCommandHandler<RequiereValidacionAlSerAsignadoCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => true;
    protected override string OperationName => "activar validacion al asignar rol";
    protected override string AlreadyMessage => "El rol ya requiere validacion al ser asignado.";
    protected override string SuccessMessage => "Validacion requerida al asignar rol activada correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.RequiereValidacionDeAsignacion;
    protected override void SetCurrentValue(Rol role, bool value) => role.RequiereValidacionDeAsignacion = value;
}
