namespace AUT2Services.Infra.Tools.EmailManager;

public sealed class MailkitSmtpClientFactory : IMailkitSmtpClientFactory
{
    public IMailkitSmtpClient Create()
    {
        return new MailkitSmtpClientAdapter();
    }
}
