namespace AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;

public class SolicitaRecuperacionClaveCommandValidations : SolicitaRecuperacionClaveValidations<SolicitaRecuperacionClaveCommand>
{
    public SolicitaRecuperacionClaveCommandValidations()
    {
        Validate_CorreoElectronico();
    }
}
