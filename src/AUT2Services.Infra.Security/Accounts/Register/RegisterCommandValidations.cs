namespace AUT2Services.Infra.Security.Accounts.Register;
public class RegisterCommandValidations : RegisterValidations<RegisterCommand>
{
    public RegisterCommandValidations()
    {
        Validate_CorreoElectronico();
        Validate_NumeroDeTelefono();
        Validate_TipoDeUsuario();
        Validate_Password();
        Validate_NumeroDeDocumento();
    }
}
