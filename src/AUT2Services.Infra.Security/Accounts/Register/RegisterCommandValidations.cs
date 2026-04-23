namespace AUT2Services.Infra.Security.Accounts.Register;
public class RegisterCommandValidations : RegisterValidations<RegisterCommand>
{
    public RegisterCommandValidations()
    {
        Validate_CorreoElectronico();
        Validate_NumeroDeTelefono();
        Validate_TipoDeUsuario();
        Validate_Nacionalidad();
        Validate_DocumentoDeIdentidad();
        Validate_Password();
        Validate_ConfirmaContraseña();
        Validate_NumeroDeDocumento();
        Validate_CodigoValidadorDocumento();
        Validate_PrimerNombre();
        Validate_PrimerApellido();
        Validate_SexoDeclarativo();
        Validate_SexoRegistral();
        Validate_FechaDeNacimiento();
        Validate_TerminosYCondiciones();
    }
}
