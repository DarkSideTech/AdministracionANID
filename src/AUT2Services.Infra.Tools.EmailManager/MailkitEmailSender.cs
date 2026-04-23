using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Authentication;

namespace AUT2Services.Infra.Tools.EmailManager;

public class MailkitEmailSender : IEmailMessageSender
{
    private const int DefaultSmtpTimeoutMilliseconds = 15000;
    private readonly SendEmailOptions sendEmailOptions;
    private readonly IMailkitSmtpClientFactory smtpClientFactory;
    private readonly ILogger<MailkitEmailSender> logger;

    public MailkitEmailSender(
        IOptions<SendEmailOptions> sendEmailOptions,
        IMailkitSmtpClientFactory smtpClientFactory,
        ILogger<MailkitEmailSender> logger)
    {
        this.sendEmailOptions = sendEmailOptions.Value;
        this.smtpClientFactory = smtpClientFactory;
        this.logger = logger;
    }

    public async Task<ResultModel> SendEmail(EmailDataModel emailData)
    {
        var result = new ResultModel()
        {
            Result = false,
            Data = string.Empty
        };

        var message = new MimeMessage();

        if (emailData.FromMailboxAddresses.Any())
        {
            foreach (var item in emailData.FromMailboxAddresses)
            {
                message.From.Add(new MailboxAddress(item.Name, item.Address));
            }
        }
        else
        {
            result.Data = "No existe una cuenta de correo origen, no es posible enviar el mensaje";
            return result;
        }

        if (emailData.ToMailboxAddresses.Any())
        {
            foreach (var item in emailData.ToMailboxAddresses)
            {
                message.To.Add(new MailboxAddress(item.Name, item.Address));
            }
        }
        else
        {
            result.Data = "No existe una cuenta de correo de destino, no es posible enviar el mensaje";
            return result;
        }

        if (!string.IsNullOrEmpty(emailData.Subject))
        {
            message.Subject = emailData.Subject;
        }
        else
        {
            result.Data = "No existe subject para el mensaje, no es posible enviar el mensaje";
            return result;
        }

        var builder = new BodyBuilder();
        if (string.Equals(emailData.BodyType, EnumEmailBodyType.HTML_BODY, StringComparison.OrdinalIgnoreCase))
        {
            builder.HtmlBody = emailData.Body;
        }
        else
        {
            builder.TextBody = emailData.Body;
        }

        message.Body = builder.ToMessageBody();

        using var client = smtpClientFactory.Create();

        try
        {
            client.Timeout = DefaultSmtpTimeoutMilliseconds;

            await client.ConnectAsync(sendEmailOptions.SmtpClient, sendEmailOptions.Port, sendEmailOptions.EnableSsl);
            await client.AuthenticateAsync(sendEmailOptions.Remitente, sendEmailOptions.Password);
            await client.SendAsync(message);

            logger.LogInformation(
                "Correo enviado via SMTP a {RecipientCount} destinatario(s) con asunto {Subject}.",
                emailData.ToMailboxAddresses.Count,
                emailData.Subject);

            result.Result = true;
            result.Data = "Correo enviado correctamente.";
        }
        catch (AuthenticationException e)
        {
            logger.LogError(
                e,
                "Error de autenticacion SMTP para el remitente configurado {Remitente}.",
                sendEmailOptions.Remitente);
            result.Data = "No fue posible autenticarse contra el servidor de correo configurado.";
        }
        catch (Exception e)
        {
            logger.LogError(
                e,
                "Error enviando correo SMTP a {Recipients}.",
                string.Join(", ", emailData.ToMailboxAddresses.Select(item => item.Address)));
            result.Data = "No fue posible enviar el correo electronico con la configuracion actual.";
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true);
            }
        }

        return result;
    }
}
