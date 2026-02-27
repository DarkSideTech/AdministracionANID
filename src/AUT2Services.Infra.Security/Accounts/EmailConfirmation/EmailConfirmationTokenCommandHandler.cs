using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;

namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public class EmailConfirmationTokenCommandHandler : CommandHandler,
    IRequestHandler<EmailConfirmationTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IMediatorHandler mediator;
    private readonly IEntidadRepository entidadRepository;
    private readonly IEmailMessageSender emailMessageSender;

    public EmailConfirmationTokenCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext aUT2ServicesContext,
        IMediatorHandler mediator,
        IEntidadRepository entidadRepository,
        IEmailMessageSender emailMessageSender)
    {
        this.userManager = userManager;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.entidadRepository = entidadRepository;
        this.emailMessageSender = emailMessageSender;
    }

    public async Task<CommandResponse> Handle(EmailConfirmationTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var usuario = await this.userManager.FindByEmailAsync(command.Email);
            if (usuario is null)
            {
                AddError("El usuario no existe, no se puede validar el correo electronico");
                return CommandResponse;
            }
            else
            {
                if(usuario.EmailConfirmed)
                {
                    AddError("El usuario ya cuenta con el correo electronico validado");
                    return CommandResponse;
                }

                var result = await this.userManager.ConfirmEmailAsync(usuario, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.ConfirmationToken)));
                if (!result.Succeeded)
                {
                    AddError("No se puede validar el correo electronico");
                    await transaction.RollbackAsync(cancellationToken);
                    return CommandResponse;
                }

                var baseEntityCommand = new BaseEntityCommand()
                {
                    CodigoOrganizacion = JsonConvert.DeserializeObject<InformacionAdicionalModel>(usuario.InformacionAdicional!)!.NumeroDeDocumento,
                    NombreOrganizacion = usuario.NombreADesplegar,
                    Id_Usuario = Guid.Parse(usuario.Id),
                    TipoDeEntidad = EnumTipoDeEntidad.PERSONA,
                    CorreoElectronico = usuario.Email,
                };

                var resulCrearBaseEntityCommand = await mediator.SendCommand(baseEntityCommand, cancellationToken);

                if (!resulCrearBaseEntityCommand.Result)
                {
                    foreach (var item in resulCrearBaseEntityCommand.ValidationResult.Errors)
                    {
                        AddError($"{item.ErrorCode} {item.ErrorMessage}");
                    }
                    await transaction.RollbackAsync(cancellationToken);
                    return CommandResponse;
                }

                if (string.IsNullOrEmpty(resulCrearBaseEntityCommand.Data))
                {
                    AddError("No se pudo crear la entidad base del usuario");
                    await transaction.RollbackAsync(cancellationToken);
                    return CommandResponse;
                }

                await transaction.CommitAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            await transaction.RollbackAsync(cancellationToken);
        }

        CommandResponse.Data = string.Empty;
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
