namespace AUT2Services.Infra.Security.Accounts.ActivarUsuario;

public class ActivarUsuarioCommandValidations : ActivarUsuarioValidations<ActivarUsuarioCommand>
{
    public ActivarUsuarioCommandValidations()
    {
        Validate_IdUsuario();
    }
}
