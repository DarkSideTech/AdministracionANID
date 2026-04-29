using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ActivaDetalleDeAutorizacionesCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<ActivaDetalleDeAutorizacionesCommandHandler> logger) : RolFlagCommandHandler<ActivaDetalleDeAutorizacionesCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => true;
    protected override string OperationName => "activar detalle de autorizaciones";
    protected override string AlreadyMessage => "El rol ya tiene activo el detalle de autorizaciones.";
    protected override string SuccessMessage => "Detalle de autorizaciones activado correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.ActivaDetalleDeAutorizaciones;
    protected override void SetCurrentValue(Rol role, bool value) => role.ActivaDetalleDeAutorizaciones = value;
}
