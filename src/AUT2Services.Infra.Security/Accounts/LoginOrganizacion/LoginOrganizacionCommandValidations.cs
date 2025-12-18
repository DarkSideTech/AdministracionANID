namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommandValidations : LoginOrganizacionValidations<LoginOrganizacionCommand>
{
    public LoginOrganizacionCommandValidations()
    {
        Validate_UserName();
        Validate_Password();
        Validate_Organizacion();
    }
}
