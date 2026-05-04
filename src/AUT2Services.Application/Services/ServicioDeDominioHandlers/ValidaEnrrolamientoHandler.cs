using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Traceability;
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

        if (command.Id_Usuario_Validado is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_Validado), "Debe informar el usuario validado."));
            return result;
        }

        if (command.Id_Usuario_Valida_Enrrolamiento is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_Valida_Enrrolamiento), "Debe informar el usuario que realiza la validacion."));
            return result;
        }

        if (!Guid.TryParse(userAccessor.GetIdUsuario(), out var idUsuarioAutenticado))
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_Valida_Enrrolamiento), "No fue posible determinar el usuario autenticado."));
            return result;
        }

        if (command.Id_Usuario_Valida_Enrrolamiento.Value != idUsuarioAutenticado)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_Valida_Enrrolamiento), "El usuario validador informado no corresponde al usuario autenticado."));
            return result;
        }

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

        if (!usuarioValidaEnrrolamiento)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_Valida_Enrrolamiento), "El usuario no tiene asignado el rol VALIDA_ENRROLAMIENTO."));
            return result;
        }

        var existUser = await userManager.Users
            .FirstOrDefaultAsync(x => x.Id == command.Id_Usuario_Validado.ToString());

        if (existUser is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"El usuario validado id [{command.Id_Usuario_Validado}] no existe, no es posible validar el enrrolamiento"));
            return result;
        }

        var usuarioValidador = await userManager.Users
            .FirstOrDefaultAsync(x => x.Id == command.Id_Usuario_Valida_Enrrolamiento.ToString());

        if (usuarioValidador is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_Valida_Enrrolamiento), $"El usuario validador id [{command.Id_Usuario_Valida_Enrrolamiento}] no existe."));
            return result;
        }

        if (string.Equals(existUser.TipoDeUsuario, EnumTipoDeUsuario.NACIONAL, StringComparison.OrdinalIgnoreCase))
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(existUser.TipoDeUsuario), "Un usuario NACIONAL no requiere validacion de enrrolamiento."));
            return result;
        }

        if (existUser.RequiereValidacionEnrrolamiento != true)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(existUser.RequiereValidacionEnrrolamiento), "El usuario no tiene validacion de enrrolamiento pendiente."));
            return result;
        }

        if (!string.Equals(existUser.EstadoDeUsuario, EnumEstadoDeUsuario.PROCESO_REGISTRO, StringComparison.OrdinalIgnoreCase))
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(existUser.EstadoDeUsuario), "El usuario no se encuentra en estado PROCESO_REGISTRO."));
            return result;
        }

        if (!existUser.EmailConfirmed)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(existUser.EmailConfirmed), "El usuario debe confirmar su correo electronico antes de validar el enrrolamiento."));
            return result;
        }

        var entidadBaseExistente = await entidadRepository.BuscarPor_Id_Usuario_TipoDeEntidad_Persona(Guid.Parse(existUser.Id));
        var usuarioValidadoAntes = UsuarioTraceabilityState.FromUser(existUser);

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            existUser.EstadoDeUsuario = EnumEstadoDeUsuario.REGISTRADO;
            existUser.RequiereValidacionEnrrolamiento = false;

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
                clock.UtcNow);

            var resultCrearValidacionEnrrolamientoCommand = await mediator.SendCommand(crearValidacionEnrrolamientoCommand, cancellationToken);
            if (!resultCrearValidacionEnrrolamientoCommand.Result)
            {
                foreach (var item in resultCrearValidacionEnrrolamientoCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }

                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            if (entidadBaseExistente is null)
            {
                var informacionAdicional = existUser.InformacionAdicional.ToInformacionAdicionalModel();
                var baseEntityCommand = new BaseEntityCommand
                {
                    CodigoOrganizacion = informacionAdicional.NumeroDeDocumento,
                    NombreOrganizacion = existUser.NombreADesplegar,
                    Id_Usuario = Guid.Parse(existUser.Id),
                    TipoDeEntidad = EnumTipoDeEntidad.PERSONA,
                    CorreoElectronico = string.Empty,
                    PermitirCorreoElectronicoVacio = true
                };

                var resultCrearBaseEntityCommand = await mediator.SendCommand(baseEntityCommand, cancellationToken);
                if (!resultCrearBaseEntityCommand.Result)
                {
                    foreach (var item in resultCrearBaseEntityCommand.ValidationResult.Errors)
                    {
                        result.ValidationResult.Errors.Add(item);
                    }

                    await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return result;
                }
            }

            securityTraceabilityService.TrackUpdate(
                SecurityTraceabilityCommandTypes.EnrrolamientoValidation,
                existUser.Id,
                SecurityTraceabilityEventTypes.EnrrolamientoValidado,
                usuarioValidadoAntes,
                UsuarioTraceabilityState.FromUser(existUser) with
                {
                    ActionContext = "ENROLLMENT_VALIDATION",
                    Result = "SUCCESS",
                    RespondedAtUtc = clock.UtcNow,
                    IdUsuarioValidado = Guid.Parse(existUser.Id),
                    IdUsuarioValidador = Guid.Parse(usuarioValidador.Id)
                });

            securityTraceabilityService.TrackCreate(
                SecurityTraceabilityCommandTypes.EnrrolamientoValidation,
                usuarioValidador.Id,
                SecurityTraceabilityEventTypes.EnrrolamientoValidacionRealizada,
                UsuarioTraceabilityState.FromUser(usuarioValidador) with
                {
                    ActionContext = "ENROLLMENT_VALIDATION",
                    Result = "SUCCESS",
                    RespondedAtUtc = clock.UtcNow,
                    IdUsuarioValidado = Guid.Parse(existUser.Id),
                    IdUsuarioValidador = Guid.Parse(usuarioValidador.Id)
                });

            if (!await context.Commit())
            {
                result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaEnrrolamiento), "No fue posible persistir la trazabilidad de la validacion de enrrolamiento."));
                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            await context.CommitExternalTransactionAsync(transaction, cancellationToken);
            result.Data = resultCrearValidacionEnrrolamientoCommand.Data;
            result.Result = true;
        }
        catch (Exception ex)
        {
            await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"Error no manejado al momento de validar el enrrolamiento, error: {ex.Message}"));
        }

        return result;
    }
}
