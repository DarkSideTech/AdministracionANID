using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AUT2Services.Infra.Tools.EmailManager;

public class MailkitEmailSender : IEmailMessageSender
{
    private readonly SendEmailOptions sendEmailOptions;

    public MailkitEmailSender(
        IOptions<SendEmailOptions> sendEmailOptions
        )
    {
        this.sendEmailOptions = sendEmailOptions.Value;
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

        builder.HtmlBody = emailData.Body;
        message.Body = builder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(sendEmailOptions.SmtpClient, sendEmailOptions.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(sendEmailOptions.Remitente, sendEmailOptions.Password);
                await client.SendAsync(message);
            }
            catch (AuthenticationException e)
            {
                result.Data = $"Authentication error: {e.Message}";
            }
            catch (Exception e)
            {
                result.Data = $"An error occurred: {e.Message}";
            }
            finally
            {
                await client.DisconnectAsync(true);
                result.Result = true;
            }
        }

        return result;
    }
}
