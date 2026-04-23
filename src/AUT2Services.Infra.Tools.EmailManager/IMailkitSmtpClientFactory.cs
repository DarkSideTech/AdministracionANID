namespace AUT2Services.Infra.Tools.EmailManager;

public interface IMailkitSmtpClientFactory
{
    IMailkitSmtpClient Create();
}
