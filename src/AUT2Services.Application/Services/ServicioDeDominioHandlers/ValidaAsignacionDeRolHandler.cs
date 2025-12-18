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

        var existPoliticaAsignada = await politicaAsignadaRepository.BuscarPor_Id((Guid)command.Id_PoliticaAsignada!);

        if (existPoliticaAsignada is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"La politica asignada id [{command.Id_PoliticaAsignada}] no existe, no es posible validar la asignacion de rol"));
            return result;
        }

        var existEntidad = await entidadRepository.BuscarPor_Id(existPoliticaAsignada.Id_Entidad);

        if (existEntidad is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"No existe la entidad id [{existPoliticaAsignada.Id_Entidad}], no es posible validar la asignacion de rol"));
            return result;
        }

        var existUnidadorganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(existEntidad.Id_UnidadOrganizacional);

        if (existUnidadorganizacional is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"No existe la unidad organizacional [{existEntidad.Id_UnidadOrganizacional}], no es posible validar la asignacion de rol"));
            return result;
        }

        var rolValidaAsignacionDeRol = await roleManager.Roles
            .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.VALIDA_ASIGNACION_ROLES);

        if (rolValidaAsignacionDeRol is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"No existe el rol que Valida la Asignacion De Roles"));
            return result;
        }

        var organizacionANID = await organizacionRepository.BuscarPor_Codigo(EnumOrganizacionBase.ANID);

        if (organizacionANID is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"La Organizacion ANID no existe, contactar co un administrador, no es posible validar la asignacion de rol"));
            return result;
        }

        var usuarioValidaAsignacionDeRol = await servicioDeDominioRepository.UsuarioConRolValidaAsignacionDeRol(
            (Guid)command.Id_Usuario_ValidaAsignacionRol!,
            Guid.Parse(rolValidaAsignacionDeRol.Id),
            existUnidadorganizacional.Id_Organizacion,
            organizacionANID.Id);

        if (usuarioValidaAsignacionDeRol)
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                existPoliticaAsignada.CambiarRolAsignadoValidado(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"Error no manejado al momento de crear una entidad nueva, error: {ex.Message}"));
            }
            result.Result = true;
            await transaction.CommitAsync(cancellationToken);
        }

        return result;
    }
}
