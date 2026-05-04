using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> SincronizarUnidadesOrganizacionalesOrganizacion(
        SincronizarUnidadesOrganizacionalesOrganizacionServicioDeDominioViewModel command)
    {
        var result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        var resultado = new SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel();

        if (!UsuarioActualEsAdministradorAnid())
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarUnidadesOrganizacionalesOrganizacion),
                "El usuario autenticado debe tener rol ADMINISTRADOR para administrar la relacion Organizacion - UnidadOrganizacional."));
            result.Data = resultado;
            return result;
        }

        if (command.Id_Organizacion is null || command.Id_Organizacion == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarUnidadesOrganizacionalesOrganizacion),
                "El Id_Organizacion es obligatorio."));
            result.Data = resultado;
            return result;
        }

        var idOrganizacionDestino = command.Id_Organizacion.Value;
        var organizacionDestino = await organizacionRepository.BuscarPor_Id(idOrganizacionDestino);
        if (organizacionDestino is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarUnidadesOrganizacionalesOrganizacion),
                $"La organizacion id [{idOrganizacionDestino}] no existe."));
            result.Data = resultado;
            return result;
        }

        var unidadesAsignar = NormalizarIds(command.UnidadesAsignar).ToArray();
        var unidadesDesasignar = NormalizarIds(command.UnidadesDesasignar).ToArray();
        var unidadesEnAmbasOperaciones = unidadesAsignar.Intersect(unidadesDesasignar).ToArray();

        if (unidadesEnAmbasOperaciones.Length != 0)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarUnidadesOrganizacionalesOrganizacion),
                $"Las unidades [{string.Join(", ", unidadesEnAmbasOperaciones)}] no pueden venir en asignar y desasignar al mismo tiempo."));
            result.Data = resultado;
            return result;
        }

        if (unidadesAsignar.Length == 0 && unidadesDesasignar.Length == 0)
        {
            result.Result = true;
            result.Data = resultado;
            return result;
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await ValidarDesasignacionesNoSoportadas(
                idOrganizacionDestino,
                unidadesDesasignar,
                resultado);

            await AsignarUnidadesOrganizacionales(
                idOrganizacionDestino,
                unidadesAsignar,
                resultado);

            if (resultado.Errores.Count != 0)
            {
                result.ValidationResult.Errors.Add(new ValidationFailure(
                    nameof(SincronizarUnidadesOrganizacionalesOrganizacion),
                    "No fue posible sincronizar todas las unidades organizacionales solicitadas."));
                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                result.Data = resultado;
                return result;
            }

            await context.SaveChangesAsync(cancellationToken);
            await context.CommitExternalTransactionAsync(transaction, cancellationToken);

            result.Result = true;
            result.Data = resultado;
            return result;
        }
        catch (Exception ex)
        {
            await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarUnidadesOrganizacionalesOrganizacion),
                $"Error no manejado al sincronizar unidades organizacionales de la organizacion [{idOrganizacionDestino}], error: {ex.Message}"));
            result.Data = resultado;
            return result;
        }
    }

    private static IEnumerable<Guid> NormalizarIds(IEnumerable<Guid>? ids)
    {
        return (ids ?? [])
            .Where(id => id != Guid.Empty)
            .Distinct();
    }

    private async Task ValidarDesasignacionesNoSoportadas(
        Guid idOrganizacionDestino,
        IReadOnlyCollection<Guid> unidadesDesasignar,
        SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel resultado)
    {
        if (unidadesDesasignar.Count == 0)
        {
            return;
        }

        var unidades = await context.UnidadOrganizacional
            .AsNoTracking()
            .Where(unidad => unidadesDesasignar.Contains(unidad.Id))
            .ToDictionaryAsync(unidad => unidad.Id, cancellationToken);

        foreach (var idUnidadOrganizacional in unidadesDesasignar)
        {
            if (!unidades.TryGetValue(idUnidadOrganizacional, out var unidad))
            {
                resultado.OmitidasPorError++;
                resultado.Errores.Add($"La unidad organizacional [{idUnidadOrganizacional}] no existe.");
                continue;
            }

            if (unidad.Id_Organizacion != idOrganizacionDestino)
            {
                resultado.OmitidasPorExistir++;
                continue;
            }

            resultado.OmitidasPorError++;
            resultado.Errores.Add(
                $"La unidad organizacional [{idUnidadOrganizacional}] no puede quedar sin organizacion. Para sacarla de esta organizacion debe asignarse a otra organizacion destino.");
        }
    }

    private async Task AsignarUnidadesOrganizacionales(
        Guid idOrganizacionDestino,
        IReadOnlyCollection<Guid> unidadesAsignar,
        SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel resultado)
    {
        if (unidadesAsignar.Count == 0)
        {
            return;
        }

        var unidades = await context.UnidadOrganizacional
            .AsTracking()
            .Where(unidad => unidadesAsignar.Contains(unidad.Id))
            .ToDictionaryAsync(unidad => unidad.Id, cancellationToken);

        foreach (var idUnidadOrganizacional in unidadesAsignar.Where(id => !unidades.ContainsKey(id)))
        {
            resultado.OmitidasPorError++;
            resultado.Errores.Add($"La unidad organizacional [{idUnidadOrganizacional}] no existe.");
        }

        var unidadesAMover = unidades.Values
            .Where(unidad => unidad.Id_Organizacion != idOrganizacionDestino)
            .ToArray();

        foreach (var unidad in unidades.Values.Where(unidad => unidad.Id_Organizacion == idOrganizacionDestino))
        {
            resultado.OmitidasPorExistir++;
        }

        foreach (var unidad in unidadesAMover.Where(unidad => !unidad.Activo))
        {
            resultado.OmitidasPorError++;
            resultado.Errores.Add($"La unidad organizacional [{unidad.Id}] no esta activa, no es posible asignarla.");
        }

        var unidadesActivasAMover = unidadesAMover
            .Where(unidad => unidad.Activo)
            .ToArray();

        if (unidadesActivasAMover.Length == 0)
        {
            return;
        }

        await ValidarOrganizacionesOrigenConUnidadRestante(unidadesActivasAMover, resultado);

        if (resultado.Errores.Count != 0)
        {
            return;
        }

        var entidadesYPoliticasEliminadas = await EliminarEntidadesYPoliticasPorUnidades(
            unidadesActivasAMover.Select(unidad => unidad.Id).ToArray(),
            resultado);

        if (!entidadesYPoliticasEliminadas)
        {
            return;
        }

        foreach (var unidad in unidadesActivasAMover)
        {
            unidad.CambiarOrganizacion(idOrganizacionDestino);
            context.UnidadOrganizacional.Update(unidad);
            resultado.Reasignadas++;
            resultado.Asignadas++;
        }
    }

    private async Task<bool> EliminarEntidadesYPoliticasPorUnidades(
        IReadOnlyCollection<Guid> idsUnidadesOrganizacionales,
        SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel resultado)
    {
        if (idsUnidadesOrganizacionales.Count == 0)
        {
            return true;
        }

        var entidades = (await entidadRepository.BuscarPor_Ids_UnidadOrganizacional(idsUnidadesOrganizacionales))
            .OrderBy(entidad => entidad.Id_UnidadOrganizacional)
            .ThenBy(entidad => entidad.Id)
            .ToArray();

        if (entidades.Length == 0)
        {
            return true;
        }

        foreach (var entidadBase in entidades.Where(entidad => entidad.EntidadBase))
        {
            resultado.OmitidasPorError++;
            resultado.Errores.Add($"La entidad [{entidadBase.Id}] asociada a la unidad organizacional [{entidadBase.Id_UnidadOrganizacional}] es EntidadBase y no se puede eliminar.");
        }

        var idsEntidades = entidades.Select(entidad => entidad.Id).ToArray();
        var politicasAsignadas = (await politicaAsignadaRepository.BuscarPor_Id_Entidades(idsEntidades))
            .OrderBy(politica => politica.Id_Entidad)
            .ThenBy(politica => politica.Id)
            .ToArray();

        foreach (var politicaAsignadaBase in politicasAsignadas.Where(politica => politica.PoliticaAsignadaBase))
        {
            resultado.OmitidasPorError++;
            resultado.Errores.Add($"La politica asignada [{politicaAsignadaBase.Id}] asociada a la entidad [{politicaAsignadaBase.Id_Entidad}] es PoliticaAsignadaBase y no se puede eliminar.");
        }

        if (resultado.Errores.Count != 0)
        {
            return false;
        }

        foreach (var politicaAsignada in politicasAsignadas)
        {
            var eliminarPoliticaAsignadaCommand = new EliminarPoliticaAsignadaCommand(
                politicaAsignada.Id,
                politicaAsignada.Id_Entidad);

            var eliminarPoliticaAsignadaResult = await mediator.SendCommand(eliminarPoliticaAsignadaCommand, cancellationToken);

            if (!eliminarPoliticaAsignadaResult.Result)
            {
                resultado.OmitidasPorError++;
                AgregarErrores(eliminarPoliticaAsignadaResult, resultado);
                return false;
            }

            resultado.PoliticasAsignadasEliminadas++;
        }

        foreach (var entidad in entidades)
        {
            var eliminarEntidadCommand = new EliminarEntidadCommand(entidad.Id);
            var eliminarEntidadResult = await mediator.SendCommand(eliminarEntidadCommand, cancellationToken);

            if (!eliminarEntidadResult.Result)
            {
                resultado.OmitidasPorError++;
                AgregarErrores(eliminarEntidadResult, resultado);
                return false;
            }

            resultado.EntidadesEliminadas++;
        }

        return true;
    }

    private async Task ValidarOrganizacionesOrigenConUnidadRestante(
        IReadOnlyCollection<Domain.Entities.UnidadOrganizacional> unidades,
        SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel resultado)
    {
        var unidadesPorOrganizacionOrigen = unidades
            .GroupBy(unidad => unidad.Id_Organizacion)
            .ToDictionary(group => group.Key, group => group.Select(unidad => unidad.Id).ToArray());

        var idsOrganizacionesOrigen = unidadesPorOrganizacionOrigen.Keys.ToArray();
        var cantidadesActualesPorOrganizacion = await context.UnidadOrganizacional
            .AsNoTracking()
            .Where(unidad => idsOrganizacionesOrigen.Contains(unidad.Id_Organizacion) && unidad.Activo)
            .GroupBy(unidad => unidad.Id_Organizacion)
            .Select(group => new
            {
                IdOrganizacion = group.Key,
                Cantidad = group.Count()
            })
            .ToDictionaryAsync(item => item.IdOrganizacion, item => item.Cantidad, cancellationToken);

        foreach (var item in unidadesPorOrganizacionOrigen)
        {
            cantidadesActualesPorOrganizacion.TryGetValue(item.Key, out var cantidadActual);
            var cantidadRestante = cantidadActual - item.Value.Length;

            if (cantidadRestante >= 1)
            {
                continue;
            }

            foreach (var idUnidadOrganizacional in item.Value)
            {
                resultado.OmitidasPorError++;
                resultado.Errores.Add(
                    $"La unidad organizacional [{idUnidadOrganizacional}] no se puede reasignar porque la organizacion origen [{item.Key}] debe conservar al menos una unidad organizacional activa.");
            }
        }
    }

    private static void AgregarErrores(
        CommandResponse commandResponse,
        SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel resultado)
    {
        foreach (var error in commandResponse.ValidationResult.Errors)
        {
            resultado.Errores.Add(error.ErrorMessage);
        }
    }
}
