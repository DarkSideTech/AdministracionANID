using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Enumerations;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> CrearOrganizacion(CrearOrganizacionServicioDeDominioViewModel command)
    {
        CommandResponse result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        var existOrganizacion = await organizacionRepository.BuscarPor_Codigo(command.Codigo!);

        if (existOrganizacion is not null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"La Organizacion Codigo [{command.Codigo}] ya existe, no es posible crearla"));
            return result;
        }

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var crearOrganizacionCommand = new CrearOrganizacionCommand(
                command.IdOrganizacion!,
                command.Codigo!,
                command.Nombre!,
                command.Descripcion!
                );

            var resultCrearOrganizacionCommand = await mediator.SendCommand(crearOrganizacionCommand, cancellationToken);

            if (!resultCrearOrganizacionCommand.Result)
            {
                foreach (var item in resultCrearOrganizacionCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }
                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            var organizacionCreada = resultCrearOrganizacionCommand.GetData<OrganizacionDTO>();

            var crearUnidadOrganizacionalCommand = new CrearUnidadOrganizacionalCommand(
                organizacionCreada!.Id,
                EnumUnidadOrganizacionalBase.CASA_MATRIZ,
                EnumUnidadOrganizacionalBase.CASA_MATRIZ,
                EnumUnidadOrganizacionalBase.CASA_MATRIZ
                );

            var resultCrearUnidadOrganizacionalCommand = await mediator.SendCommand(crearUnidadOrganizacionalCommand, cancellationToken);

            if (!resultCrearOrganizacionCommand.Result)
            {
                foreach (var item in resultCrearOrganizacionCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }
                    await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }
        }
        catch (Exception ex)
        {
            await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"Error no manejado al momento de crear una entidad nueva, error: {ex.Message}"));
        }

        result.Result = true;
        await context.CommitExternalTransactionAsync(transaction, cancellationToken);
        return result;
    }
}
