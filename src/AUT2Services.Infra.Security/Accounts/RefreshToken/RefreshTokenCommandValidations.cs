namespace AUT2Services.Infra.Security.Accounts.RefreshToken;

public class RefreshTokenCommandValidations : RefreshTokenValidations<RefreshTokenCommand>
{
    public RefreshTokenCommandValidations()
    {
        Validate_RefreshToken();
    }
}