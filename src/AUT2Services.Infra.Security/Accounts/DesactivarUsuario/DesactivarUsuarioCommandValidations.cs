namespace AUT2Services.Infra.Security.Accounts.DesactivarUsuario;

public class DesactivarUsuarioCommandValidations : DesactivarUsuarioValidations<DesactivarUsuarioCommand>
{
    public DesactivarUsuarioCommandValidations()
    {
        Validate_IdUsuario();
    }
}
