namespace AUT2Services.Infra.Security.Accounts.ReSendEmailConfirmation;

public class ResendEmailConfirmationTokenCommandValidations : ResendEmailConfirmationTokenValidations<ResendEmailConfirmationTokenCommand>
{
    public ResendEmailConfirmationTokenCommandValidations()
    {
        Validate_Email();
    }
}