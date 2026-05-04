using AUT2Services.Application.ViewModels.ServiciosDeDominio;
using AUT2Services.Application.Extensions;
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Enumerations;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<CommandResponse> CrearEntidad(CrearEntidadServicioDeDominioViewModel command)
    {
        CommandResponse result = new CommandResponse
        {
            Result = false
        };
        result.ValidationResult.Errors = [];

        if (command.Id_UnidadOrganizacional is null || command.Id_UnidadOrganizacional == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), "El Id_UnidadOrganizacional es obligatorio para crear la entidad"));
            return result;
        }

        if (command.Id_Usuario is null || command.Id_Usuario == Guid.Empty)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), "El Id_Usuario es obligatorio para crear la entidad"));
            return result;
        }

        var idUnidadOrganizacional = command.Id_UnidadOrganizacional.Value;
        var idUsuario = command.Id_Usuario.Value;

        var existUnidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(idUnidadOrganizacional);

        if (existUnidadOrganizacional is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"La unidad organizaciona id [{idUnidadOrganizacional}] no existe, no es posible crear la entidad"));
            return result;
        }

        if (!await UsuarioActualPuedeOperarUnidadOrganizacional(
                existUnidadOrganizacional,
                nameof(CrearEntidad),
                result.ValidationResult.Errors))
        {
            return result;
        }

        var existUser = await userManager.Users
                .FirstOrDefaultAsync(x => x.Id == idUsuario.ToString());

        if (existUser is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"El usuario id [{idUsuario}] no existe, no es posible crear la entidad"));
            return result;
        }

        var existEntidad = await entidadRepository.BuscarPor_Id_Usuario_Id_UnidadOrganizacional(
            idUsuario,
            idUnidadOrganizacional);

        if (existEntidad is not null)
        {
            result.Result = true;
            result.Data = existEntidad.ToViewModel();
            return result;
        }

        var existProceso = await procesoRepository.BuscarPor_Codigo(EnumProcesosBase.ADMINISTRACION);

        if (existProceso is null)
        {
            result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"El proceso de administracion no existe, no es posible crear la entidad"));
            return result;
        }

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var crearEntidadCommand = new CrearEntidadCommand(
                idUnidadOrganizacional,
                idUsuario,
                command.TipoDeEntidad!,
                command.CorreoElectronico!
                );

            var resultCrearEntidadCommand = await mediator.SendCommand(crearEntidadCommand, cancellationToken);

            if (!resultCrearEntidadCommand.Result)
            {
                foreach (var item in resultCrearEntidadCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }
                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            var entidadCreada = resultCrearEntidadCommand.GetData<EntidadDTO>();

            var existEntidadPrincipal = await servicioDeDominioRepository.BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(idUsuario, idUnidadOrganizacional);

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
                    await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return result;
                }
            }

            var cambiaEntidadAPrincipalEntidadCommand = new CambiaEntidadAPrincipalEntidadCommand(
                    entidadCreada!.Id
                    );

            var resultCambiaEntidadAPrincipalEntidadCommand = await mediator.SendCommand(cambiaEntidadAPrincipalEntidadCommand, cancellationToken);

            if (!resultCambiaEntidadAPrincipalEntidadCommand.Result)
            {
                foreach (var item in resultCambiaEntidadAPrincipalEntidadCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }
                await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            var rol = await roleManager.Roles
                .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.ADMINISTRADOR_ENTIDAD);

            if (rol is null)
            {
                result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"No existe el rol Administrador de entidad"));
                return result;
            }

            var crearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand = new CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand(
                entidadCreada.Id,
                Guid.Parse(rol.Id),
                existProceso.Id
                );

            var resultCrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand = await mediator.SendCommand(crearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand, cancellationToken);

            if (!resultCrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand.Result)
            {
                foreach (var item in resultCrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand.ValidationResult.Errors)
                {
                    result.ValidationResult.Errors.Add(item);
                }
                    await context.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return result;
            }

            rol = await roleManager.Roles
                .FirstOrDefaultAsync(x => x.Name == EnumRolesBase.VALIDA_ASIGNACION_ROLES);

            if (rol is null)
            {
                result.ValidationResult.Errors.Add(new ValidationFailure(nameof(CrearEntidad), $"No existe el rol que Valida la Asignacion De Roles"));
                return result;
            }

            crearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand = new CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand(
                entidadCreada.Id,
                Guid.Parse(rol.Id),
                existProceso.Id
                );

            resultCrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand = await mediator.SendCommand(crearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand, cancellationToken);

            if (!resultCrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand.Result)
            {
                foreach (var item in resultCrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand.ValidationResult.Errors)
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
