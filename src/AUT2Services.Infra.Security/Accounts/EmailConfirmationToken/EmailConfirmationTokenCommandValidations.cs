namespace AUT2Services.Infra.Security.Accounts.EmailConfirmationToken;

public class EmailConfirmationTokenCommandValidations : EmailConfirmationTokenValidations<EmailConfirmationTokenCommand>
{
    public EmailConfirmationTokenCommandValidations()
    {
        Validate_UserId();
        Validate_Token();
    }
}
