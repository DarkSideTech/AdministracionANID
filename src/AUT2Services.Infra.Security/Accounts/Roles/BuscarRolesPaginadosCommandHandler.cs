using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Security.Accounts.Roles;

public class BuscarRolesPaginadosCommandHandler(
    RoleManager<Rol> roleManager,
    ICsrfService csrfService,
    ICurrentUserService currentUserService,
    ISessionValidationService sessionValidationService) : RolCommandHandlerBase(roleManager, csrfService, currentUserService, sessionValidationService),
    IRequestHandler<BuscarRolesPaginadosCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(BuscarRolesPaginadosCommand command, CancellationToken cancellationToken)
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

        var numeroDePagina = command.NumeroDePagina.GetValueOrDefault(1);
        var cantidadPorPagina = command.CantidadPorPagina.GetValueOrDefault(10);
        var busqueda = command.Busqueda?.Trim();

        var query = RoleManager.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var busquedaNormalizada = busqueda.ToUpperInvariant();
            var incluyeVerdaderos = MatchesBooleanSearch(busquedaNormalizada, true);
            var incluyeFalsos = MatchesBooleanSearch(busquedaNormalizada, false);

            query = query.Where(rol =>
                (rol.Id ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (rol.Name ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (rol.NormalizedName ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (rol.Descripcion ?? string.Empty).ToUpper().Contains(busquedaNormalizada) ||
                (incluyeVerdaderos && (rol.ActivaDetalleDeAutorizaciones == true
                    || rol.RequiereValidacionDeAsignacion == true
                    || rol.ValidaAsignacionDeRoles == true
                    || rol.ValidaEnrrolamiento == true
                    || rol.Activo == true)) ||
                (incluyeFalsos && (rol.ActivaDetalleDeAutorizaciones != true
                    || rol.RequiereValidacionDeAsignacion != true
                    || rol.ValidaAsignacionDeRoles != true
                    || rol.ValidaEnrrolamiento != true
                    || rol.Activo != true)));
        }

        var total = await query.LongCountAsync(cancellationToken);
        var roles = await query
            .OrderBy(rol => rol.NormalizedName ?? rol.Name)
            .ThenBy(rol => rol.Id)
            .Skip((numeroDePagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .Select(rol => new RolPaginadoItemResponse(
                rol.Id,
                rol.Name,
                rol.NormalizedName,
                rol.Descripcion,
                rol.ActivaDetalleDeAutorizaciones,
                rol.RequiereValidacionDeAsignacion,
                rol.ValidaAsignacionDeRoles,
                rol.ValidaEnrrolamiento,
                rol.RolBase,
                rol.Activo))
            .ToArrayAsync(cancellationToken);

        CommandResponse.Data = new BuscarRolesPaginadosResponse(
            numeroDePagina,
            cantidadPorPagina,
            total,
            roles);
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private static bool MatchesBooleanSearch(string value, bool expected)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return expected
            ? value is "TRUE" or "VERDADERO" or "ACTIVO" or "ACTIVA" or "SI"
            : value is "FALSE" or "FALSO" or "INACTIVO" or "INACTIVA" or "NO";
    }
}
