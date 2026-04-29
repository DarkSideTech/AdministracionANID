using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class DesactivaDetalleDeAutorizacionesCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService,
    ILogger<DesactivaDetalleDeAutorizacionesCommandHandler> logger) : RolFlagCommandHandler<DesactivaDetalleDeAutorizacionesCommand>(
        roleManager,
        csrfService,
        currentUserService,
        sessionValidationService,
        logger)
{
    protected override bool DesiredValue => false;
    protected override string OperationName => "desactivar detalle de autorizaciones";
    protected override string AlreadyMessage => "El rol ya tiene desactivado el detalle de autorizaciones.";
    protected override string SuccessMessage => "Detalle de autorizaciones desactivado correctamente.";
    protected override bool? GetCurrentValue(Rol role) => role.ActivaDetalleDeAutorizaciones;
    protected override void SetCurrentValue(Rol role, bool value) => role.ActivaDetalleDeAutorizaciones = value;
}
