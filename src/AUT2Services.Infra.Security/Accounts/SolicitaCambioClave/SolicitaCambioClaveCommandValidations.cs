namespace AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;

public class SolicitaCambioClaveCommandValidations : SolicitaCambioClaveValidations<SolicitaCambioClaveCommand>
{
    public SolicitaCambioClaveCommandValidations()
    {
        Validate_IdUsuario();
        Validate_ClaveActual();
        Validate_NuevaClave();
        Validate_ConfirmaNuevaClave();
    }
}
