using AUT2Services.Domain.Core.Enumerations;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Tools.EmailManager;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Authentication;

namespace AUT2Services.Tests.Email;

[TestClass]
public class MailkitEmailSenderTests
{
    [TestMethod]
    public async Task SendEmail_WhenAuthenticationFails_ReturnsFailure()
    {
        var fakeClient = new FakeMailkitSmtpClient
        {
            AuthenticateException = new AuthenticationException("bad credentials")
        };

        var sender = CreateSender(fakeClient);
        var email = CreateEmail();

        var result = await sender.SendEmail(email);

        Assert.IsFalse(result.Result);
        Assert.AreEqual("No fue posible autenticarse contra el servidor de correo configurado.", result.Data);
    }

    [TestMethod]
    public async Task SendEmail_WhenSendSucceeds_ReturnsSuccess()
    {
        var fakeClient = new FakeMailkitSmtpClient();
        var sender = CreateSender(fakeClient);
        var email = CreateEmail();

        var result = await sender.SendEmail(email);

        Assert.IsTrue(result.Result);
        Assert.AreEqual("Correo enviado correctamente.", result.Data);
        Assert.IsNotNull(fakeClient.SentMessage);
        Assert.AreEqual("Asunto de prueba", fakeClient.SentMessage!.Subject);
        Assert.IsTrue(fakeClient.DisconnectCalled);
    }

    [TestMethod]
    public async Task SendEmail_WhenBodyTypeIsText_UsesTextBody()
    {
        var fakeClient = new FakeMailkitSmtpClient();
        var sender = CreateSender(fakeClient);
        var email = CreateEmail();
        email.BodyType = EnumEmailBodyType.TEXT_BODY;

        var result = await sender.SendEmail(email);

        Assert.IsTrue(result.Result);
        var textPart = fakeClient.SentMessage!.Body as TextPart;
        Assert.IsNotNull(textPart);
        Assert.AreEqual("Texto de prueba", textPart.Text);
    }

    private static MailkitEmailSender CreateSender(FakeMailkitSmtpClient fakeClient)
    {
        return new MailkitEmailSender(
            Options.Create(new SendEmailOptions
            {
                SmtpClient = "smtp.gmail.com",
                Port = 587,
                Remitente = "sender@example.com",
                Password = "secret",
                EnableSsl = true
            }),
            new FakeMailkitSmtpClientFactory(fakeClient),
            NullLogger<MailkitEmailSender>.Instance);
    }

    private static EmailDataModel CreateEmail()
    {
        return new EmailDataModel
        {
            Subject = "Asunto de prueba",
            Body = "Texto de prueba",
            BodyType = EnumEmailBodyType.HTML_BODY,
            FromMailboxAddresses =
            [
                new AddressMailbox { Name = "Origen", Address = "sender@example.com" }
            ],
            ToMailboxAddresses =
            [
                new AddressMailbox { Name = "Destino", Address = "target@example.com" }
            ]
        };
    }

    private sealed class FakeMailkitSmtpClientFactory(FakeMailkitSmtpClient client) : IMailkitSmtpClientFactory
    {
        public IMailkitSmtpClient Create()
        {
            return client;
        }
    }

    private sealed class FakeMailkitSmtpClient : IMailkitSmtpClient
    {
        public int Timeout { get; set; }
        public bool IsConnected { get; private set; }
        public Exception? ConnectException { get; set; }
        public Exception? AuthenticateException { get; set; }
        public Exception? SendException { get; set; }
        public MimeMessage? SentMessage { get; private set; }
        public bool DisconnectCalled { get; private set; }

        public Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken = default)
        {
            if (ConnectException is not null)
            {
                throw ConnectException;
            }

            IsConnected = true;
            return Task.CompletedTask;
        }

        public Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = default)
        {
            if (AuthenticateException is not null)
            {
                throw AuthenticateException;
            }

            return Task.CompletedTask;
        }

        public Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
        {
            if (SendException is not null)
            {
                throw SendException;
            }

            SentMessage = message;
            return Task.CompletedTask;
        }

        public Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default)
        {
            DisconnectCalled = true;
            IsConnected = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
