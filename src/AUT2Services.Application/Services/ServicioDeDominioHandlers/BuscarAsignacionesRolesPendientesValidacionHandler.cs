using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Enumerations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> BuscarAsignacionesRolesPendientesValidacion(BuscarAsignacionesRolesPendientesValidacionServicioDeDominioViewModel command)
    {
        var result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        if (!Guid.TryParse(userAccessor.GetIdUsuario(), out var idUsuarioActual))
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(BuscarAsignacionesRolesPendientesValidacion), "No fue posible determinar el usuario autenticado."));
            return result;
        }

        var rolValidaAsignacionDeRol = await roleManager.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.VALIDA_ASIGNACION_ROLES);

        if (rolValidaAsignacionDeRol is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(BuscarAsignacionesRolesPendientesValidacion), "No existe el rol VALIDA_ASIGNACION_ROLES."));
            return result;
        }

        var organizacionANID = await organizacionRepository.BuscarPor_Codigo(EnumOrganizacionBase.ANID);

        if (organizacionANID is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(BuscarAsignacionesRolesPendientesValidacion), "La organizacion ANID no existe. No es posible determinar el alcance de validacion."));
            return result;
        }

        var numeroDePagina = Math.Max(command.NumeroDePagina.GetValueOrDefault(1), 1);
        var cantidadPorPagina = Math.Clamp(command.CantidadPorPagina.GetValueOrDefault(10), 1, 100);

        var page = await servicioDeDominioRepository.BuscarAsignacionesRolesPendientesValidacion(
            idUsuarioActual,
            Guid.Parse(rolValidaAsignacionDeRol.Id),
            organizacionANID.Id,
            numeroDePagina,
            cantidadPorPagina,
            command.Busqueda,
            clock.UtcNow);

        result.Data = new BuscarAsignacionesRolesPendientesValidacionResponse(
            page.NumeroDePagina,
            page.CantidadPorPagina,
            page.Total,
            page.Items.Select(MapAsignacionRolPendienteValidacion).ToArray());
        result.Result = true;
        return result;
    }

    private static AsignacionRolPendienteValidacionViewModel MapAsignacionRolPendienteValidacion(AsignacionRolPendienteValidacionDTO item)
    {
        return new AsignacionRolPendienteValidacionViewModel(
            item.IdPoliticaAsignada,
            item.IdEntidad,
            item.IdUsuario,
            item.NombreUsuario,
            item.CorreoElectronico,
            item.IdOrganizacion,
            item.CodigoOrganizacion,
            item.NombreOrganizacion,
            item.IdUnidadOrganizacional,
            item.CodigoUnidadOrganizacional,
            item.NombreUnidadOrganizacional,
            item.TipoDeEntidad,
            item.IdRol,
            item.NombreRol,
            item.IdProceso,
            item.CodigoProceso,
            item.NombreProceso,
            item.FechaCreacion,
            item.FechaInicioAsignacion,
            item.FechaTerminoAsignacion,
            item.RolRequiereValidacion,
            item.RolAsignadoValidado);
    }
}
