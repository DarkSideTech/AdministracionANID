namespace AUT2Services.Infra.Security.Accounts.Roles;

public class ActivaDetalleDeAutorizacionesCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<ActivaDetalleDeAutorizacionesCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
