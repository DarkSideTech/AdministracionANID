namespace AUT2Services.Infra.Security.Accounts.AdminModificaCorreoElectronico;

public class AdminModificaCorreoElectronicoCommandValidations : AdminModificaCorreoElectronicoValidations<AdminModificaCorreoElectronicoCommand>
{
    public AdminModificaCorreoElectronicoCommandValidations()
    {
        Validate_IdUsuario();
        Validate_NuevoCorreoElectronico();
    }
}
