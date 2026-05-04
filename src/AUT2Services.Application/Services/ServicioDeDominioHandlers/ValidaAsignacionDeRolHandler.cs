using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Enumerations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> ValidaAsignacionDeRol(ValidaAsignacionDeRolServicioDeDominioViewModel command)
    {
        CommandResponse result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        if (!command.Id_PoliticaAsignada.HasValue || command.Id_PoliticaAsignada == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_PoliticaAsignada), "Debe informar la politica asignada a validar."));
            return result;
        }

        if (!command.Id_Usuario_ValidaAsignacionRol.HasValue || command.Id_Usuario_ValidaAsignacionRol == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_ValidaAsignacionRol), "Debe informar el usuario que valida la asignacion de rol."));
            return result;
        }

        var existPoliticaAsignada = await politicaAsignadaRepository.BuscarPor_Id((Guid)command.Id_PoliticaAsignada!);

        if (existPoliticaAsignada is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaAsignacionDeRol), $"La politica asignada id [{command.Id_PoliticaAsignada}] no existe, no es posible validar la asignacion de rol."));
            return result;
        }

        if (!existPoliticaAsignada.RolRequiereValidacion)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(existPoliticaAsignada.RolRequiereValidacion), "La politica asignada no requiere validacion de asignacion de rol."));
            return result;
        }

        if (existPoliticaAsignada.RolAsignadoValidado)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(existPoliticaAsignada.RolAsignadoValidado), "La politica asignada ya se encuentra validada."));
            return result;
        }

        var existEntidad = await entidadRepository.BuscarPor_Id(existPoliticaAsignada.Id_Entidad);

        if (existEntidad is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaAsignacionDeRol), $"No existe la entidad id [{existPoliticaAsignada.Id_Entidad}], no es posible validar la asignacion de rol."));
            return result;
        }

        var existUnidadorganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(existEntidad.Id_UnidadOrganizacional);

        if (existUnidadorganizacional is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaAsignacionDeRol), $"No existe la unidad organizacional [{existEntidad.Id_UnidadOrganizacional}], no es posible validar la asignacion de rol."));
            return result;
        }

        var rolAsignado = await roleManager.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == existPoliticaAsignada.Id_Rol.ToString());

        if (rolAsignado is null || rolAsignado.RequiereValidacionDeAsignacion != true)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(rolAsignado.RequiereValidacionDeAsignacion), "El rol asignado no requiere validacion de asignacion."));
            return result;
        }

        var rolValidaAsignacionDeRol = await roleManager.Roles
            .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.VALIDA_ASIGNACION_ROLES);

        if (rolValidaAsignacionDeRol is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaAsignacionDeRol), "No existe el rol VALIDA_ASIGNACION_ROLES."));
            return result;
        }

        var organizacionANID = await organizacionRepository.BuscarPor_Codigo(EnumOrganizacionBase.ANID);

        if (organizacionANID is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaAsignacionDeRol), "La Organizacion ANID no existe, no es posible validar la asignacion de rol."));
            return result;
        }

        var usuarioValidaAsignacionDeRol = await servicioDeDominioRepository.UsuarioConRolValidaAsignacionDeRol(
            (Guid)command.Id_Usuario_ValidaAsignacionRol!,
            Guid.Parse(rolValidaAsignacionDeRol.Id),
            existUnidadorganizacional.Id_Organizacion,
            organizacionANID.Id);

        if (!usuarioValidaAsignacionDeRol)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(command.Id_Usuario_ValidaAsignacionRol), "El usuario no tiene asignado el rol VALIDA_ASIGNACION_ROLES para la organizacion de la asignacion."));
            return result;
        }

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            existPoliticaAsignada.CambiarRolAsignadoValidado(true);
            politicaAsignadaRepository.Modificar(existPoliticaAsignada);
            await context.SaveChangesAsync(cancellationToken);
            await context.CommitExternalTransactionAsync(transaction, cancellationToken);
            result.Result = true;
        }
        catch (Exception ex)
        {
            await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(ValidaAsignacionDeRol), $"Error no manejado al validar la asignacion de rol, error: {ex.Message}"));
        }

        return result;
    }
}
