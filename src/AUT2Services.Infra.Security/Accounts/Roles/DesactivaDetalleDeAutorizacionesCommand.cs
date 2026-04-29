namespace AUT2Services.Infra.Security.Accounts.Roles;

public class DesactivaDetalleDeAutorizacionesCommand : RolIdCommand
{
    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new RolIdCommandValidations<DesactivaDetalleDeAutorizacionesCommand>().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
