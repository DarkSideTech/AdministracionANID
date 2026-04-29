namespace AUT2Services.Infra.Security.Accounts.Roles;

public class DesactivaValidacionDeAsignacionDeRolesCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<DesactivaValidacionDeAsignacionDeRolesCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
