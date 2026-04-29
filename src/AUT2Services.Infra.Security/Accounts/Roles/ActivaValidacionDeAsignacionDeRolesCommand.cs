namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ActivaValidacionDeAsignacionDeRolesCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<ActivaValidacionDeAsignacionDeRolesCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
