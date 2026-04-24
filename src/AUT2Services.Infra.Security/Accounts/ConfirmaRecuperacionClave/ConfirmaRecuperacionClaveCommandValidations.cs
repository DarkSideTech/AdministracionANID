namespace AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;

public class ConfirmaRecuperacionClaveCommandValidations : ConfirmaRecuperacionClaveValidations<ConfirmaRecuperacionClaveCommand>
{
    public ConfirmaRecuperacionClaveCommandValidations()
    {
        Validate_CorreoElectronico();
        Validate_CodigoValidacion();
        Validate_NuevaClave();
        Validate_ConfirmaNuevaClave();
    }
}
