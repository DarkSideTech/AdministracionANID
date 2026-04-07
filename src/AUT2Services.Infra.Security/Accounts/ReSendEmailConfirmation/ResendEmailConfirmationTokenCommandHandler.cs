using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AUT2Services.Infra.Security.Accounts.ReSendEmailConfirmation;

public class ResendEmailConfirmationTokenCommandHandler : CommandHandler,
    IRequestHandler<ResendEmailConfirmationTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IMediatorHandler mediator;
    private readonly ICsrfService csrfService;
    private readonly IEmailConfirmationThrottleService emailConfirmationThrottleService;
    private readonly IEmailMessageSender emailSender;
    private readonly SendEmailOptions sendEmailOptions;

    public ResendEmailConfirmationTokenCommandHandler(
        UserManager<Usuario> userManager,
        AUT2ServicesContext aUT2ServicesContext,
        IMediatorHandler mediator,
        IOptions<SendEmailOptions> sendEmailOptions,
        ICsrfService csrfService,
        IEmailConfirmationThrottleService emailConfirmationThrottleService,
        IEmailMessageSender emailSender)
    {
        this.userManager = userManager;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.mediator = mediator;
        this.csrfService = csrfService;
        this.emailConfirmationThrottleService = emailConfirmationThrottleService;
        this.emailSender = emailSender;
        this.sendEmailOptions = sendEmailOptions.Value;
    }

    public async Task<CommandResponse> Handle(ResendEmailConfirmationTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Data = "Si la cuenta existe y el correo electrónico está pendiente de confirmación, se enviará un nuevo correo electrónico de confirmación.";
        CommandResponse.Result = true;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        if (!csrfService.IsRequestValid(command.Request))
        {
            AddError("Invalid CSRF token.");
            return CommandResponse;
        }

        var normalizedEmail = command.Email.Trim();

        var usuario = await userManager.FindByEmailAsync(normalizedEmail);
        if (usuario is null || usuario.EmailConfirmed)
        {
            return CommandResponse;
        }

        if (emailConfirmationThrottleService.CanSend(normalizedEmail, out _))
        {
            return CommandResponse;
        }

        var emailToken = await this.userManager.GenerateEmailConfirmationTokenAsync(usuario);
        var confirmationEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));
        var confirmationId = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(usuario.Id));

        var emailConfirmationUrl = QueryHelpers.AddQueryString(sendEmailOptions.APIValidateEmail, new Dictionary<string, string?>
        {
            ["userId"] = confirmationId,
            ["token"] = confirmationEmailToken
        });

        string emailBody = string.Format($@"
                        <!DOCTYPE html>
                        <html lang=""es"">
	                        <head>
		                        <meta charset=""UTF-8"">
		                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
		                        <title>Correo Automático</title>
	                        </head>
	                        <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;"">
		                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
			                        <tr>
				                        <td align=""center"" style=""padding: 20px 0;"">
					                        <table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);"">
						                        <tr>
							                        <td align=""center"">
                                                        <h1 style=""color: #333333; margin: 0; font-size: 24px;"">Agencia Nacional de Investigacion y Desarrollo</h1>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td style=""padding: 40px; text-align: center;"">
								                        <h2 style=""color: #333333; margin: 0; font-size: 24px;"">Hola Nuevo Usuario</h2>
								                        <div style=""margin-top: 30px; padding: 20px; background-color: #f8f9fa; border: 2px dashed #007bff; display: inline-block;"">
									                        <span style=""font-size: 18px; font-weight: bold; color: #007bff; letter-spacing: 2px;"">
										                        <a href=""{sendEmailOptions.APIValidateEmail}?validationtoken={emailConfirmationUrl}"">Link para Validar Cuenta de Correo</a>. 
									                        </span>
								                        </div>
							                        </td>
						                        </tr>
						                        <tr>
							                        <td style=""padding: 10px; background-color: #333333; color: #ffffff; text-align: center; font-size: 12px;"">
								                        <p style=""margin: 0;"">&copy; 2026 Dark Side Tech. Todos los derechos reservados.</p>
							                        </td>
						                        </tr>
					                        </table>
				                        </td>
			                        </tr>
		                        </table>
	                        </body>
                        </html>");

        var email = new EmailDataModel()
        {
            FromMailboxAddresses = [new() { Address = sendEmailOptions.Remitente, Name = "Correo ANID" }],
            ToMailboxAddresses = [new() { Address = usuario.Email!, Name = usuario.NombreADesplegar! }],
            Body = emailBody,
            BodyType = EnumEmailBodyType.HTML_BODY,
            Subject = "ANID: Validación Registro Usuario"
        };

        var result = await emailSender.SendEmail(email);

        CommandResponse.Data = JsonConvert.SerializeObject(new EmailConfirmationTokenViewModel()
        {
            Id = confirmationId!,
            ConfirmationToken = confirmationEmailToken
        });
        CommandResponse.Result = true;
        return CommandResponse;
    }
}
