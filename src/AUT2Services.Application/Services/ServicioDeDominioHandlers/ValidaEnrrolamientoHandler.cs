using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Enumerations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> ValidaEnrrolamiento(ValidaEnrrolamientoServicioDeDominioViewModel command)
    {
        CommandResponse result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        var rolValidaEnrrolamiento = await roleManager.Roles
            .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.VALIDA_ENRROLAMIENTO);

        if (rolValidaEnrrolamiento is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"No existe el rol que Valida el enrrolamiento"));
            return result;
        }

        var usuarioValidaEnrrolamiento = await servicioDeDominioRepository.UsuarioConRolValidaEnrrolamiento(
            (Guid)command.Id_Usuario_Valida_Enrrolamiento!,
            Guid.Parse(rolValidaEnrrolamiento.Id));

        if (usuarioValidaEnrrolamiento)
        {
            var existUser = await userManager.Users
                .FirstOrDefaultAsync(x => x.Id == command.Id_Usuario_Validado.ToString());

            if (existUser is null)
            {
                result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"El usuario validado id [{command.Id_Usuario_Validado}] no existe, no es posible validar el enrrolamiento"));
                return result;
            }

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                existUser.EstadoDeUsuario = EnumEstadoDeUsuario.REGISTRADO;

                var resultUpdateUser = await userManager.UpdateAsync(existUser);
                if (!resultUpdateUser.Succeeded)
                {
                    result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"El usuario id [{command.Id_Usuario_Validado}] no es posible validar el enrrolamiento"));
                    await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return result;
                }

                var crearValidacionEnrrolamientoCommand = new CrearValidacionEnrrolamientoCommand(
                    Guid.Parse(existUser.Id),
                    (Guid)command.Id_Usuario_Valida_Enrrolamiento!,
                    true,
                    clock.UtcNow
                    );
            }
            catch (Exception ex)
            {
                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"Error no manejado al momento de crear una entidad nueva, error: {ex.Message}"));
            }
            result.Result = true;
            await context.CommitExternalTransactionAsync(transaction, cancellationToken);
        }

        return result;
    }
}
