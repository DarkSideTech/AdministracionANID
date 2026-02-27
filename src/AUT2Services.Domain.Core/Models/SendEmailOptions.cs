namespace AUT2Services.Domain.Core.Models;

public class SendEmailOptions
{
    public const string EmailOptionsKey = "SendEmailOptions";

    public string SmtpClient { get; set; } = "smtp.office365.com";
    public int Port { get; set; } = 587;
    public string Remitente { get; set; } = "colaborador@dark-side.tech";
    public string Password { get; set; } = "C4nt1ct4.";
    public bool EnableSsl { get; set; } = true;
    public string APIValidateEmail { get; set; } = string.Empty;
    public string URLEmailValidate { get; set; } = string.Empty;
    public string URLEmailNotValidate { get; set; } = string.Empty;
}
