using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Traceability;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Accounts.EmailConfirmationToken;

public class EmailConfirmationTokenCommandHandler : CommandHandler,
    IRequestHandler<EmailConfirmationTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IMediatorHandler mediator;
    private readonly ICsrfService csrfService;
    private readonly IEntidadRepository entidadRepository;
    private readonly ISecurityTraceabilityService securityTraceabilityService;

    public EmailConfirmationTokenCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext aUT2ServicesContext,
        IMediatorHandler mediator,
        ICsrfService csrfService,
        IEntidadRepository entidadRepository,
        ISecurityTraceabilityService securityTraceabilityService)
    {
        this.userManager = userManager;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.csrfService = csrfService;
        this.entidadRepository = entidadRepository;
        this.securityTraceabilityService = securityTraceabilityService;
    }

    public async Task<CommandResponse> Handle(EmailConfirmationTokenCommand command, CancellationToken cancellationToken)
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

        if (string.IsNullOrWhiteSpace(command.UserId) || string.IsNullOrWhiteSpace(command.Token))
        {
            AddError("Se requieren el ID de usuario y el token.");
            return CommandResponse;
        }

        var decodedUserId = DecodeUserId(command.UserId);
        if (string.IsNullOrWhiteSpace(decodedUserId))
        {
            AddError("Solicitud de confirmacion de correo electronico no valida.");
            return CommandResponse;
        }

        var usuario = await userManager.FindByIdAsync(decodedUserId);
        if (usuario is null)
        {
            AddError("Solicitud de confirmacion de correo electronico no valida.");
            return CommandResponse;
        }

        var entidadBaseExistente = await entidadRepository.BuscarPor_Id_Usuario_TipoDeEntidad_Persona(Guid.Parse(usuario.Id));
        var confirmationEventType = entidadBaseExistente is null
            ? SecurityTraceabilityEventTypes.CorreoCuentaValidado
            : SecurityTraceabilityEventTypes.CorreoCambioValidado;
        if (usuario.EmailConfirmed && entidadBaseExistente is not null)
        {
            CommandResponse.Data = "El correo electronico ya habia sido confirmado.";
            CommandResponse.Result = true;
            return CommandResponse;
        }

        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Token));
        }
        catch (FormatException)
        {
            AddError("Token de confirmacion de correo electronico no valido.");
            return CommandResponse;
        }

        using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (!usuario.EmailConfirmed)
            {
                var confirmEmail = await userManager.ConfirmEmailAsync(usuario, decodedToken);
                if (!confirmEmail.Succeeded)
                {
                    AddError("Token de confirmacion de correo electronico no valido o caducado.");
                    await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return CommandResponse;
                }
            }

            if (entidadBaseExistente is null && usuario.RequiereValidacionEnrrolamiento == true)
            {
                securityTraceabilityService.TrackCreate(
                    command,
                    usuario.Id,
                    confirmationEventType,
                    UsuarioTraceabilityState.FromUser(usuario) with
                    {
                        ActionContext = "REGISTER_CONFIRMATION",
                        RequestPath = command.Request.Path,
                        Result = "PENDING_ENROLLMENT_VALIDATION",
                        RespondedAtUtc = DateTimeOffset.UtcNow
                    });

                if (!await aUT2ServicesContext.Commit())
                {
                    AddError("No fue posible persistir la trazabilidad de la validacion del correo electronico.");
                    await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return CommandResponse;
                }

                await aUT2ServicesContext.CommitExternalTransactionAsync(transaction, cancellationToken);
                csrfService.EnsureTokenCookie(command.Response);
                CommandResponse.Data = "Correo electronico confirmado. Tu cuenta queda pendiente de validacion de enrrolamiento.";
                CommandResponse.Result = true;
                return CommandResponse;
            }

            if (entidadBaseExistente is null)
            {
                var baseEntityCommand = new BaseEntityCommand()
                {
                    CodigoOrganizacion = JsonSerializer.Deserialize<InformacionAdicionalModel>(usuario.InformacionAdicional!)!.NumeroDeDocumento,
                    NombreOrganizacion = usuario.NombreADesplegar,
                    Id_Usuario = Guid.Parse(usuario.Id),
                    TipoDeEntidad = EnumTipoDeEntidad.PERSONA,
                    CorreoElectronico = string.Empty,
                    PermitirCorreoElectronicoVacio = true,
                };

                var resulCrearBaseEntityCommand = await mediator.SendCommand(baseEntityCommand, cancellationToken);

                if (!resulCrearBaseEntityCommand.Result)
                {
                    foreach (var item in resulCrearBaseEntityCommand.ValidationResult.Errors)
                    {
                        AddError($"{item.ErrorCode} {item.ErrorMessage}");
                    }

                    await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return CommandResponse;
                }

                if (!resulCrearBaseEntityCommand.HasData)
                {
                    AddError("No se pudo crear la entidad base del usuario");
                    await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                    return CommandResponse;
                }
            }

            securityTraceabilityService.TrackCreate(
                command,
                usuario.Id,
                confirmationEventType,
                UsuarioTraceabilityState.FromUser(usuario) with
                {
                    ActionContext = entidadBaseExistente is null ? "REGISTER_CONFIRMATION" : "EMAIL_CHANGE_CONFIRMATION",
                    RequestPath = command.Request.Path,
                    Result = "SUCCESS",
                    RespondedAtUtc = DateTimeOffset.UtcNow
                });

            if (!await aUT2ServicesContext.Commit())
            {
                AddError("No fue posible persistir la trazabilidad de la validacion del correo electronico.");
                await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
                return CommandResponse;
            }

            await aUT2ServicesContext.CommitExternalTransactionAsync(transaction, cancellationToken);
            csrfService.EnsureTokenCookie(command.Response);
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de confirmar el correo electronico, message [{ex.Message}]");
            await aUT2ServicesContext.RollbackExternalTransactionAsync(transaction, cancellationToken);
            return CommandResponse;
        }

        CommandResponse.Data = "Correo electronico confirmado. Ya puedes continuar con el uso de la plataforma.";
        CommandResponse.Result = true;
        return CommandResponse;
    }

    private static string DecodeUserId(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return string.Empty;
        }

        try
        {
            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userId));
        }
        catch (FormatException)
        {
            return userId;
        }
    }
}
