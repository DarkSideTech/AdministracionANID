namespace AUT2Services.Infra.Security.Accounts.BaseEntity;

public class BaseEntityCommandValidations : BaseEntityValidations<BaseEntityCommand>
{
    public BaseEntityCommandValidations()
    {
        Validate_CodigoOrganizacion();
        Validate_NombreOrganizacion();
        Validate_Id_Usuario();
        Validate_TipoDeEntidad();
        Validate_CorreoElectronico();
    }
}
