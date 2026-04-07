namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public class EmailConfirmationTokenCommandValidations : EmailConfirmationTokenValidations<EmailConfirmationTokenCommand>
{
    public EmailConfirmationTokenCommandValidations()
    {
        Validate_UserId();
        Validate_Token();
    }
}
