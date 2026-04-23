namespace AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;

public class ConfirmaCambioClaveCommandValidations : ConfirmaCambioClaveValidations<ConfirmaCambioClaveCommand>
{
    public ConfirmaCambioClaveCommandValidations()
    {
        Validate_IdUsuario();
        Validate_ClaveActual();
        Validate_NuevaClave();
        Validate_ConfirmaNuevaClave();
        Validate_CodigoValidacion();
    }
}
