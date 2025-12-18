using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Core.Commands;
using FluentValidation.Results;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> MarcarEntidadComoPrincipal(MarcarEntidadComoPrincipalServicioDeDominioViewModel command)
    {
        CommandResponse result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        var existEntidad = await entidadRepository.BuscarPor_Id((Guid)command.Id_Entidad!);

        if (existEntidad is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"La entidad id [{command.Id_Entidad}] no existe, no es posible crearla"));
            return result;
        }

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var existEntidadPrincipal = await servicioDeDominioRepository.BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(existEntidad.Id_Usuario, existEntidad.Id_UnidadOrganizacional);

            if (existEntidadPrincipal is not null)
            {
                var cambiaEntidadANoPrincipalEntidadCommand = new CambiaEntidadANoPrincipalEntidadCommand(
                    existEntidadPrincipal.Id
                    );

                var resultCambiaEntidadANoPrincipalEntidadCommand = await mediator.SendCommand(cambiaEntidadANoPrincipalEntidadCommand, cancellationToken);

                if (!resultCambiaEntidadANoPrincipalEntidadCommand.Result)
                {
                    foreach (var item in resultCambiaEntidadANoPrincipalEntidadCommand.ValidationResult.Errors)
                    {
                        result.ValidationResult.Errors.Add(item);
                    }
                    await transaction.RollbackAsync(cancellationToken);
                    return result;
                }
            }

            var cambiaEntidadAPrincipalEntidadCommand = new CambiaEntidadAPrincipalEntidadCommand(
                    existEntidad.Id
                    );

            var resultCambiaEntidadAPrincipalEntidadCommand = await mediator.SendCommand(cambiaEntidadAPrincipalEntidadCommand, cancellationToken);

            if (!resultCambiaEntidadAPrincipalEntidadCommand.Result)
            {
                foreach (var item in resultCambiaEntidadAPrincipalEntidadCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }
                await transaction.RollbackAsync(cancellationToken);
                return result;
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"Error no manejado al momento de crear una entidad nueva, error: {ex.Message}"));
        }

        result.Result = true;
        await transaction.CommitAsync(cancellationToken);
        return result;
    }
}
