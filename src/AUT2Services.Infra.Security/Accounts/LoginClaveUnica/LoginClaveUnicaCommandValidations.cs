namespace AUT2Services.Infra.Security.Accounts.LoginClaveUnica;

public class LoginClaveUnicaCommandValidations : LoginClaveUnicaValidations<LoginClaveUnicaCommand>
{
    public LoginClaveUnicaCommandValidations()
    {
        Validate_Code();
        Validate_State();
    }
}
