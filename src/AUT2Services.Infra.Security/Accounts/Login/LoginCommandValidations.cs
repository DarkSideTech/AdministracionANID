namespace AUT2Services.Infra.Security.Accounts.Login;

public class LoginCommandValidations : LoginValidations<LoginCommand>
{
    public LoginCommandValidations()
    {
        Validate_UserName();
        Validate_Password();
    }
}
