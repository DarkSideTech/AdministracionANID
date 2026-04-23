using MimeKit;

namespace AUT2Services.Infra.Tools.EmailManager;

public interface IMailkitSmtpClient : IDisposable
{
    int Timeout { get; set; }
    bool IsConnected { get; }
    Task ConnectAsync(string host, int port, bool useSsl, CancellationToken cancellationToken = default);
    Task AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = default);
    Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default);
    Task DisconnectAsync(bool quit, CancellationToken cancellationToken = default);
}
