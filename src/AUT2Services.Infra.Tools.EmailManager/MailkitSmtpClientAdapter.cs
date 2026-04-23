using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AUT2Services.Infra.Tools.EmailManager;

internal sealed class MailkitSmtpClientAdapter : IMailkitSmtpClient
{
    private readonly SmtpClient smtpClient = new();

    public int Timeout
    {
        get => smtpClient.Timeout;
        set => smtpClient.Timeout = value;
    }

    public bool IsConnected => smtpClient.IsConnected;

    public Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken = default)
    {
        var socketOptions = useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
        return smtpClient.ConnectAsync(host, port, socketOptions, cancellationToken);
    }

    public Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        return smtpClient.AuthenticateAsync(userName, password, cancellationToken);
    }

    public Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default)
    {
        return smtpClient.SendAsync(message, cancellationToken);
    }

    public Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default)
    {
        return smtpClient.DisconnectAsync(quit, cancellationToken);
    }

    public void Dispose()
    {
        smtpClient.Dispose();
    }
}
