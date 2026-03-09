namespace AUT2Services.Infra.Security.Accounts.ValidateEmail;

public class EmailConfirmationTokenCommandValidations : EmailConfirmationTokenValidations<EmailConfirmationTokenCommand>
{
    public EmailConfirmationTokenCommandValidations()
    {
        Validate_Id();
        Validate_ConfirmationToken();
    }
}
