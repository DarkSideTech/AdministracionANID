namespace AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;

public class CambioUnidadOrganizacionalEntidadRolCommandValidations : CambioUnidadOrganizacionalEntidadRolValidations<CambioUnidadOrganizacionalEntidadRolCommand>
{
    public CambioUnidadOrganizacionalEntidadRolCommandValidations()
    {
        Validate_Id_Entidad();
        Validate_Id_Rol();
    }
}
