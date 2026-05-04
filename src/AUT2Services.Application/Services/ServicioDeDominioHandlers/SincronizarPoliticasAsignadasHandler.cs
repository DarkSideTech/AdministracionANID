using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> SincronizarPoliticasAsignadas(SincronizarPoliticasAsignadasServicioDeDominioViewModel command)
    {
        var result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        var resultado = new SincronizarPoliticasAsignadasResultadoViewModel();
        var accion = command.Accion?.Trim().ToUpperInvariant();

        if (accion is not ("CREAR" or "ELIMINAR"))
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarPoliticasAsignadas),
                "La accion es obligatoria y debe ser CREAR o ELIMINAR."));
            result.Data = resultado;
            return result;
        }

        var solicitudes = NormalizarSolicitudes(command).ToArray();
        if (solicitudes.Length == 0)
        {
            result.Result = true;
            result.Data = resultado;
            return result;
        }

        var entidades = (await entidadRepository.BuscarPor_Ids(solicitudes.Select(item => item.IdEntidad)))
            .ToDictionary(item => item.Id);
        var entidadesNoAutorizadas = await ResolverEntidadesNoAutorizadas(entidades, resultado);

        var politicasAsignadasExistentes = (await politicaAsignadaRepository.BuscarPor_Id_Entidades(entidades.Keys))
            .GroupBy(item => new PoliticaAsignadaClave(item.Id_Entidad, item.Id_Rol, item.Id_Proceso))
            .ToDictionary(item => item.Key, item => item.ToArray());

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            foreach (var solicitud in solicitudes)
            {
                if (!entidades.ContainsKey(solicitud.IdEntidad))
                {
                    resultado.OmitidasPorError++;
                    resultado.Errores.Add($"La entidad [{solicitud.IdEntidad}] no existe.");
                    continue;
                }

                if (entidadesNoAutorizadas.Contains(solicitud.IdEntidad))
                {
                    resultado.OmitidasPorError++;
                    resultado.Errores.Add($"El usuario autenticado no puede operar sobre la entidad [{solicitud.IdEntidad}].");
                    continue;
                }

                var clave = new PoliticaAsignadaClave(solicitud.IdEntidad, solicitud.IdRol, solicitud.IdProceso);

                if (accion == "CREAR")
                {
                    await CrearPoliticaAsignadaSiNoExiste(solicitud, clave, politicasAsignadasExistentes, resultado);
                    continue;
                }

                await EliminarPoliticaAsignadaSiExiste(clave, politicasAsignadasExistentes, resultado);
            }

            result.Result = true;
            result.Data = resultado;
            await context.CommitExternalTransactionAsync(transaction, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(
                nameof(SincronizarPoliticasAsignadas),
                $"Error no manejado al sincronizar politicas asignadas, error: {ex.Message}"));
            result.Data = resultado;
            return result;
        }
    }

    private static IEnumerable<PoliticaAsignadaSolicitud> NormalizarSolicitudes(
        SincronizarPoliticasAsignadasServicioDeDominioViewModel command)
    {
        return (command.Items ?? [])
            .Where(entidad => entidad.IdEntidad is not null && entidad.IdEntidad != Guid.Empty)
            .SelectMany(entidad => (entidad.Politicas ?? [])
                .Where(politica =>
                    politica.IdRol is not null &&
                    politica.IdRol != Guid.Empty &&
                    politica.IdProceso is not null &&
                    politica.IdProceso != Guid.Empty)
                .Select(politica => new PoliticaAsignadaSolicitud(
                    entidad.IdEntidad!.Value,
                    politica.IdRol!.Value,
                    politica.IdProceso!.Value,
                    politica.RolRequiereValidacion == true)))
            .GroupBy(item => new PoliticaAsignadaClave(item.IdEntidad, item.IdRol, item.IdProceso))
            .Select(item => item.First());
    }

    private async Task<HashSet<Guid>> ResolverEntidadesNoAutorizadas(
        IDictionary<Guid, Domain.Entities.Entidad> entidades,
        SincronizarPoliticasAsignadasResultadoViewModel resultado)
    {
        var entidadesNoAutorizadas = new HashSet<Guid>();

        foreach (var entidad in entidades.Values)
        {
            var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional);
            if (unidadOrganizacional is null)
            {
                entidadesNoAutorizadas.Add(entidad.Id);
                resultado.Errores.Add($"La unidad organizacional asociada a la entidad [{entidad.Id}] no existe.");
                continue;
            }

            var authorizationErrors = new List<ValidationFailure>();
            var puedeOperar = await UsuarioActualPuedeOperarUnidadOrganizacional(
                unidadOrganizacional,
                nameof(SincronizarPoliticasAsignadas),
                authorizationErrors);

            if (puedeOperar)
            {
                continue;
            }

            entidadesNoAutorizadas.Add(entidad.Id);
            foreach (var error in authorizationErrors)
            {
                resultado.Errores.Add(error.ErrorMessage);
            }
        }

        return entidadesNoAutorizadas;
    }

    private async Task CrearPoliticaAsignadaSiNoExiste(
        PoliticaAsignadaSolicitud solicitud,
        PoliticaAsignadaClave clave,
        IDictionary<PoliticaAsignadaClave, Domain.Entities.PoliticaAsignada[]> politicasAsignadasExistentes,
        SincronizarPoliticasAsignadasResultadoViewModel resultado)
    {
        if (politicasAsignadasExistentes.ContainsKey(clave))
        {
            resultado.OmitidasPorExistir++;
            return;
        }

        var crearPoliticaAsignadaCommand = new CrearPoliticaAsignadaCommand(
            solicitud.IdEntidad,
            solicitud.IdRol,
            solicitud.IdProceso,
            solicitud.RolRequiereValidacion);

        var crearPoliticaAsignadaResult = await mediator.SendCommand(crearPoliticaAsignadaCommand, cancellationToken);
        if (!crearPoliticaAsignadaResult.Result)
        {
            resultado.OmitidasPorError++;
            AgregarErrores(crearPoliticaAsignadaResult, resultado);
            return;
        }

        resultado.Creadas++;
        politicasAsignadasExistentes[clave] = [];
    }

    private async Task EliminarPoliticaAsignadaSiExiste(
        PoliticaAsignadaClave clave,
        IDictionary<PoliticaAsignadaClave, Domain.Entities.PoliticaAsignada[]> politicasAsignadasExistentes,
        SincronizarPoliticasAsignadasResultadoViewModel resultado)
    {
        if (!politicasAsignadasExistentes.TryGetValue(clave, out var politicasAsignadas) || politicasAsignadas.Length == 0)
        {
            resultado.OmitidasPorNoExistir++;
            return;
        }

        foreach (var politicaAsignada in politicasAsignadas)
        {
            var eliminarPoliticaAsignadaCommand = new EliminarPoliticaAsignadaCommand(
                politicaAsignada.Id,
                clave.IdEntidad);

            var eliminarPoliticaAsignadaResult = await mediator.SendCommand(eliminarPoliticaAsignadaCommand, cancellationToken);
            if (!eliminarPoliticaAsignadaResult.Result)
            {
                resultado.OmitidasPorError++;
                AgregarErrores(eliminarPoliticaAsignadaResult, resultado);
                continue;
            }

            resultado.Eliminadas++;
        }

        politicasAsignadasExistentes.Remove(clave);
    }

    private static void AgregarErrores(
        CommandResponse commandResponse,
        SincronizarPoliticasAsignadasResultadoViewModel resultado)
    {
        foreach (var error in commandResponse.ValidationResult.Errors)
        {
            resultado.Errores.Add(error.ErrorMessage);
        }
    }

    private sealed record PoliticaAsignadaSolicitud(
        Guid IdEntidad,
        Guid IdRol,
        Guid IdProceso,
        bool RolRequiereValidacion);

    private sealed record PoliticaAsignadaClave(
        Guid IdEntidad,
        Guid IdRol,
        Guid IdProceso);
}
