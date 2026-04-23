using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Accounts.ModificaUsuario;

public class ModificaUsuarioCommandHandler : CommandHandler,
    IRequestHandler<ModificaUsuarioCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext dbContext;
    private readonly ICsrfService csrfService;
    private readonly ICurrentUserService currentUserService;
    private readonly ISessionValidationService sessionValidationService;
    private readonly ISecurityTraceabilityService securityTraceabilityService;
    private readonly ILogger<ModificaUsuarioCommandHandler> logger;

    public ModificaUsuarioCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext dbContext,
        ICsrfService csrfService,
        ICurrentUserService currentUserService,
        ISessionValidationService sessionValidationService,
        ISecurityTraceabilityService securityTraceabilityService,
        ILogger<ModificaUsuarioCommandHandler> logger)
    {
        this.userManager = userManager;
        this.dbContext = dbContext;
        this.csrfService = csrfService;
        this.currentUserService = currentUserService;
        this.sessionValidationService = sessionValidationService;
        this.securityTraceabilityService = securityTraceabilityService;
        this.logger = logger;
    }

    public async Task<CommandResponse> Handle(ModificaUsuarioCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!csrfService.IsRequestValid(command.Request))
        {
            AddError("Invalid CSRF token.");
            return CommandResponse;
        }

        if (!currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(currentUserService.UserId))
        {
            AddError("Usuario no autorizado.");
            return CommandResponse;
        }

        var principal = currentUserService.GetClaimsPrincipal(cancellationToken);
        var securityStamp = principal?.FindFirst(EnumTokenValidationClaims.SecurityStamp)?.Value;
        var isSessionValid = await sessionValidationService.IsSessionValidAsync(
            currentUserService.UserId,
            currentUserService.SessionId,
            securityStamp,
            cancellationToken);

        if (!isSessionValid)
        {
            AddError("La sesion del usuario no es valida.");
            return CommandResponse;
        }

        if (!string.Equals(currentUserService.UserId, command.IdUsuario, StringComparison.Ordinal))
        {
            AddError("No tienes permisos para modificar este usuario.");
            return CommandResponse;
        }

        var existingUser = await userManager.FindByIdAsync(command.IdUsuario!);
        if (existingUser is null)
        {
            AddError("El usuario no existe.");
            return CommandResponse;
        }

        if (!existingUser.EmailConfirmed)
        {
            AddError("El usuario tiene la validacion pendiente del correo electronico.");
            return CommandResponse;
        }

        if (existingUser.Activo != true)
        {
            AddError("El usuario no se encuentra activo.");
            return CommandResponse;
        }

        if (!string.Equals(existingUser.EstadoDeUsuario, EnumEstadoDeUsuario.REGISTRADO, StringComparison.OrdinalIgnoreCase))
        {
            AddError("El usuario no se encuentra en estado REGISTRADO.");
            return CommandResponse;
        }

        if (existingUser.UsuarioBase == true)
        {
            AddError("El usuario base no puede ser modificado.");
            return CommandResponse;
        }

        var informacionActual = existingUser.InformacionAdicional.ToInformacionAdicionalModel();
        var tipoDeUsuarioActual = (existingUser.TipoDeUsuario ?? string.Empty).Trim().ToUpperInvariant();
        var esUsuarioNacional = string.Equals(tipoDeUsuarioActual, EnumTipoDeUsuario.NACIONAL, StringComparison.OrdinalIgnoreCase);
        var beforeState = UsuarioTraceabilityState.FromUser(existingUser);

        var informacionActualizada = esUsuarioNacional
            ? BuildNationalUserInformation(informacionActual, command)
            : BuildForeignUserInformation(informacionActual, command);

        if (!ValidateUpdatedInformation(informacionActualizada, tipoDeUsuarioActual))
        {
            return CommandResponse;
        }

        existingUser.InformacionAdicional = informacionActualizada.ToJson();
        existingUser.NombreADesplegar = BuildDisplayName(
            esUsuarioNacional ? informacionActual.PrimerNombre : informacionActualizada.PrimerNombre,
            esUsuarioNacional ? informacionActual.PrimerApellido : informacionActualizada.PrimerApellido,
            existingUser.NombreADesplegar);
        existingUser.PhoneNumber = esUsuarioNacional
            ? existingUser.PhoneNumber
            : CoalesceTrimmed(command.NumeroDeTelefono, existingUser.PhoneNumber);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var updateResult = await userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    AddError($"{error.Code} - {error.Description}");
                }

                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            securityTraceabilityService.TrackUpdate(
                command,
                existingUser.Id,
                SecurityTraceabilityEventTypes.UsuarioModificado,
                beforeState,
                UsuarioTraceabilityState.FromUser(existingUser) with
                {
                    ActionContext = "PROFILE_UPDATE",
                    RequestPath = command.Request.Path
                });

            if (!await dbContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad de la modificacion del usuario.");
                await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await dbContext.CommitExternalTransactionAsync(transaction, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No fue posible modificar el usuario {UserId}.", command.IdUsuario);
            await dbContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            AddError("No fue posible modificar el usuario en este momento.");
            return CommandResponse;
        }

        CommandResponse.Data = new ModificaUsuarioResponse(
            existingUser.Id,
            "Los datos del usuario fueron actualizados correctamente.");
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private InformacionAdicionalModel BuildNationalUserInformation(
        InformacionAdicionalModel informacionActual,
        ModificaUsuarioCommand command)
    {
        return new InformacionAdicionalModel
        {
            Nacionalidad = informacionActual.Nacionalidad,
            DocumentoDeIdentidad = informacionActual.DocumentoDeIdentidad,
            NumeroDeDocumento = informacionActual.NumeroDeDocumento,
            CodigoValidadorDocumento = informacionActual.CodigoValidadorDocumento,
            PrimerNombre = informacionActual.PrimerNombre,
            SegundoNombre = informacionActual.SegundoNombre,
            PrimerApellido = informacionActual.PrimerApellido,
            SegundoApellido = informacionActual.SegundoApellido,
            SexoDeclarativo = informacionActual.SexoDeclarativo,
            SexoRegistral = command.SexoRegistral?.Trim() ?? informacionActual.SexoRegistral,
            FechaDeNacimiento = informacionActual.FechaDeNacimiento,
            TerminosYCondiciones = informacionActual.TerminosYCondiciones
        };
    }

    private InformacionAdicionalModel BuildForeignUserInformation(
        InformacionAdicionalModel informacionActual,
        ModificaUsuarioCommand command)
    {
        return new InformacionAdicionalModel
        {
            Nacionalidad = CoalesceTrimmed(command.Nacionalidad, informacionActual.Nacionalidad),
            DocumentoDeIdentidad = CoalesceTrimmed(command.DocumentoDeIdentidad, informacionActual.DocumentoDeIdentidad),
            NumeroDeDocumento = CoalesceTrimmed(command.NumeroDeDocumento, informacionActual.NumeroDeDocumento),
            CodigoValidadorDocumento = CoalesceTrimmed(command.CodigoValidadorDocumento, informacionActual.CodigoValidadorDocumento),
            PrimerNombre = CoalesceTrimmed(command.PrimerNombre, informacionActual.PrimerNombre),
            SegundoNombre = CoalesceTrimmedNullable(command.SegundoNombre, informacionActual.SegundoNombre),
            PrimerApellido = CoalesceTrimmed(command.PrimerApellido, informacionActual.PrimerApellido),
            SegundoApellido = CoalesceTrimmedNullable(command.SegundoApellido, informacionActual.SegundoApellido),
            SexoDeclarativo = CoalesceTrimmed(command.SexoDeclarativo, informacionActual.SexoDeclarativo),
            SexoRegistral = CoalesceTrimmed(command.SexoRegistral, informacionActual.SexoRegistral),
            FechaDeNacimiento = command.FechaDeNacimiento ?? informacionActual.FechaDeNacimiento,
            TerminosYCondiciones = informacionActual.TerminosYCondiciones
        };
    }

    private bool ValidateUpdatedInformation(InformacionAdicionalModel informacionActualizada, string tipoDeUsuarioActual)
    {
        if (!CommonValidator.EnumerationValidator(typeof(EnumSexoRegistral), informacionActualizada.SexoRegistral))
        {
            AddError("El valor ingresado para el campo SexoRegistral debe ser un valor valido definido en la enumeracion.");
            return false;
        }

        if (string.Equals(tipoDeUsuarioActual, EnumTipoDeUsuario.NACIONAL, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.Nacionalidad)
            || !CommonValidator.EnumerationValidator(typeof(EnumNacionalidad), informacionActualizada.Nacionalidad))
        {
            AddError("El valor ingresado para el campo Nacionalidad debe ser un valor valido definido en la enumeracion.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.DocumentoDeIdentidad)
            || !CommonValidator.EnumerationValidator(typeof(EnumDocumentoDeIdentidad), informacionActualizada.DocumentoDeIdentidad))
        {
            AddError("El valor ingresado para el campo DocumentoDeIdentidad debe ser un valor valido definido en la enumeracion.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.NumeroDeDocumento) || informacionActualizada.NumeroDeDocumento.Length is < 5 or > 100)
        {
            AddError("El valor ingresado para el campo NumeroDeDocumento debe contener entre 5 y 100 caracteres.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.CodigoValidadorDocumento) || informacionActualizada.CodigoValidadorDocumento.Length is < 1 or > 100)
        {
            AddError("El valor ingresado para el campo CodigoValidadorDocumento debe contener entre 1 y 100 caracteres.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.PrimerNombre) || informacionActualizada.PrimerNombre.Length is < 2 or > 100)
        {
            AddError("El valor ingresado para el campo PrimerNombre debe contener entre 2 y 100 caracteres.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.PrimerApellido) || informacionActualizada.PrimerApellido.Length is < 2 or > 100)
        {
            AddError("El valor ingresado para el campo PrimerApellido debe contener entre 2 y 100 caracteres.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(informacionActualizada.SexoDeclarativo)
            || !CommonValidator.EnumerationValidator(typeof(EnumSexoDeclarativo), informacionActualizada.SexoDeclarativo))
        {
            AddError("El valor ingresado para el campo SexoDeclarativo debe ser un valor valido definido en la enumeracion.");
            return false;
        }

        if (!informacionActualizada.FechaDeNacimiento.HasValue)
        {
            AddError("El valor ingresado para el campo FechaDeNacimiento no puede estar vacio.");
            return false;
        }

        return true;
    }

    private static string BuildDisplayName(string? primerNombre, string? primerApellido, string? fallbackValue)
    {
        var nombre = $"{primerNombre?.Trim()} {primerApellido?.Trim()}".Trim();
        return string.IsNullOrWhiteSpace(nombre) ? fallbackValue ?? string.Empty : nombre;
    }

    private static string? CoalesceTrimmed(string? incomingValue, string? existingValue)
    {
        var normalizedIncoming = incomingValue?.Trim();
        return string.IsNullOrWhiteSpace(normalizedIncoming) ? existingValue : normalizedIncoming;
    }

    private static string? CoalesceTrimmedNullable(string? incomingValue, string? existingValue)
    {
        return incomingValue is null ? existingValue : incomingValue.Trim();
    }
}
