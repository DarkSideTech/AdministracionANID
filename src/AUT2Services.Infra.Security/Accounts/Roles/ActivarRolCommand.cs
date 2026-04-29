namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ActivarRolCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<ActivarRolCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
