namespace AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;

public class ModificaCorreoElectronicoCommandValidations : ModificaCorreoElectronicoValidations<ModificaCorreoElectronicoCommand>
{
    public ModificaCorreoElectronicoCommandValidations()
    {
        Validate_IdUsuario();
        Validate_NuevoCorreoElectronico();
    }
}
