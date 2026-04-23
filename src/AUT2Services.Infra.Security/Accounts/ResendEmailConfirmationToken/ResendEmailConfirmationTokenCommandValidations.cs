namespace AUT2Services.Infra.Security.Accounts.ResendEmailConfirmationToken;

public class ResendEmailConfirmationTokenCommandValidations : ResendEmailConfirmationTokenValidations<ResendEmailConfirmationTokenCommand>
{
    public ResendEmailConfirmationTokenCommandValidations()
    {
        Validate_Email();
    }
}