namespace AUT2Services.Infra.Security.Accounts.LoginOrganizacion;

public class LoginOrganizacionCommandValidations : LoginOrganizacionValidations<LoginOrganizacionCommand>
{
    public LoginOrganizacionCommandValidations()
    {
        Validate_Organizacion();
    }
}
