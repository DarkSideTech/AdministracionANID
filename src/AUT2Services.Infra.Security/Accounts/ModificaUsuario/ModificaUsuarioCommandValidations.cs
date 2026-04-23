using AUT2Services.Infra.Security.Accounts.ModificaUsuario;

namespace AUT2Services.Infra.Security.Accounts.ModificaUsuario;
public class ModificaUsuarioCommandValidations : ModificaUsuarioValidations<ModificaUsuarioCommand>
{
    public ModificaUsuarioCommandValidations()
    {
        Validate_IdUsuario();
        Validate_CorreoElectronico();
        Validate_NumeroDeTelefono();
        Validate_TipoDeUsuario();
        Validate_Nacionalidad();
        Validate_DocumentoDeIdentidad();
        Validate_NumeroDeDocumento();
        Validate_CodigoValidadorDocumento();
        Validate_PrimerNombre();
        Validate_PrimerApellido();
        Validate_SexoDeclarativo();
        Validate_SexoRegistral();
        Validate_FechaDeNacimiento();
    }
}
