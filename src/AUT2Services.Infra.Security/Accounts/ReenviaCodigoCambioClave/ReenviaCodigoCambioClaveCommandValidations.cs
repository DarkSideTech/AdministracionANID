namespace AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;

public class ReenviaCodigoCambioClaveCommandValidations : ReenviaCodigoCambioClaveValidations<ReenviaCodigoCambioClaveCommand>
{
    public ReenviaCodigoCambioClaveCommandValidations()
    {
        Validate_IdUsuario();
    }
}
