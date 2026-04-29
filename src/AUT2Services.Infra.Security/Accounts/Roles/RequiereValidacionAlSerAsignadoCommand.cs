namespace AUT2Services.Infra.Security.Accounts.Roles;

public class RequiereValidacionAlSerAsignadoCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<RequiereValidacionAlSerAsignadoCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
