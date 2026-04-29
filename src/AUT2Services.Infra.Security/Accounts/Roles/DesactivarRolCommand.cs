namespace AUT2Services.Infra.Security.Accounts.Roles;

public class DesactivarRolCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<DesactivarRolCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
