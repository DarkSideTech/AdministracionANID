namespace AUT2Services.Infra.Security.Accounts.Roles;

public class NoRequiereValidacionAlSerAsignadoCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<NoRequiereValidacionAlSerAsignadoCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
