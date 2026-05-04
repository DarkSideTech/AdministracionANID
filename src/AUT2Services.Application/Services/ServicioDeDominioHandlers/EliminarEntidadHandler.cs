using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> EliminarEntidad(EliminarEntidadServicioDeDominioViewModel command)
    {
        var result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        if (command.IdEntidad is null || command.IdEntidad == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(EliminarEntidad), "El IdEntidad es obligatorio para eliminar la entidad"));
            return result;
        }

        var idEntidad = command.IdEntidad.Value;
        var existEntidad = await entidadRepository.BuscarPor_Id(idEntidad);

        if (existEntidad is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(EliminarEntidad), $"La entidad id [{idEntidad}] no existe, no es posible eliminarla"));
            return result;
        }

        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(existEntidad.Id_UnidadOrganizacional);
        if (unidadOrganizacional is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(EliminarEntidad), $"La unidad organizacional asociada a la entidad [{idEntidad}] no existe, no es posible eliminarla"));
            return result;
        }

        if (!await UsuarioActualPuedeOperarUnidadOrganizacional(
                unidadOrganizacional,
                nameof(EliminarEntidad),
                result.ValidationResult.Errors))
        {
            return result;
        }

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var politicasAsignadas = (await politicaAsignadaRepository.BuscarPor_Id_Entidad(idEntidad)).ToArray();

            foreach (var politicaAsignada in politicasAsignadas)
            {
                var eliminarPoliticaAsignadaCommand = new EliminarPoliticaAsignadaCommand(
                    politicaAsignada.Id,
                    idEntidad);

                var eliminarPoliticaAsignadaResult = await mediator.SendCommand(eliminarPoliticaAsignadaCommand, cancellationToken);

                if (!eliminarPoliticaAsignadaResult.Result)
                {
                    foreach (var item in eliminarPoliticaAsignadaResult.ValidationResult.Errors)
                    {
                        result.ValidationResult.Errors.Add(item);
                    }

                    await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return result;
                }
            }

            var eliminarEntidadCommand = new EliminarEntidadCommand(idEntidad);
            var eliminarEntidadResult = await mediator.SendCommand(eliminarEntidadCommand, cancellationToken);

            if (!eliminarEntidadResult.Result)
            {
                foreach (var item in eliminarEntidadResult.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }

                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            result.Result = true;
            result.Data = new
            {
                IdEntidad = idEntidad,
                PoliticasAsignadasEliminadas = politicasAsignadas.Length
            };

            await context.CommitExternalTransactionAsync(transaction, cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(EliminarEntidad), $"Error no manejado al momento de eliminar la entidad [{idEntidad}], error: {ex.Message}"));
            return result;
        }
    }
}
