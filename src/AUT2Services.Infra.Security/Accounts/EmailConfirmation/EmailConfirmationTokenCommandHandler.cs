using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Accounts.BaseEntity;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public class EmailConfirmationTokenCommandHandler : CommandHandler,
    IRequestHandler<EmailConfirmationTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IMediatorHandler mediator;
    private readonly ICsrfService csrfService;
    private readonly SendEmailOptions sendEmailOptions;

    public EmailConfirmationTokenCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext aUT2ServicesContext,
        IMediatorHandler mediator,
        IOptions<SendEmailOptions> sendEmailOptions,
        ICsrfService csrfService)
    {
        this.userManager = userManager;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.csrfService = csrfService;
        this.sendEmailOptions = sendEmailOptions.Value;
    }

    public async Task<CommandResponse> Handle(EmailConfirmationTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;
        CommandResponse.Data = sendEmailOptions.URLEmailNotValidate;

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

        Usuario? usuario = null;
        var user = await userManager.FindByIdAsync(command.UserId!);
        if (user is null)
        {
            AddError("Solicitud de confirmación de correo electrónico no válida.");
            return CommandResponse;
        }
        else
        {
            usuario = user;
        }

        if (usuario.EmailConfirmed)
        {
            AddError("El usuario ya cuenta con el correo electronico validado");
            return CommandResponse;
        }

        string decodedToken;
        try
        {
            decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(command.Token));
        }
        catch (FormatException)
        {
            AddError("Token de confirmación de correo electrónico no válido.");
            return CommandResponse;
        }

        using var transaction = await aUT2ServicesContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var confirmEmail = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (!confirmEmail.Succeeded)
            {
                AddError("Token de confirmación de correo electrónico no válido o caducado.");
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

            csrfService.EnsureTokenCookie(command.Response);
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            await transaction.RollbackAsync(cancellationToken);
        }

        CommandResponse.Data = "Correo electrónico confirmado. Ya puedes iniciar sesión.";
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
