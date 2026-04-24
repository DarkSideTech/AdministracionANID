namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoRecuperacionClave;

public class ReenviaCodigoRecuperacionClaveCommandValidations : ReenviaCodigoRecuperacionClaveValidations<ReenviaCodigoRecuperacionClaveCommand>
{
    public ReenviaCodigoRecuperacionClaveCommandValidations()
    {
        Validate_CorreoElectronico();
    }
}
