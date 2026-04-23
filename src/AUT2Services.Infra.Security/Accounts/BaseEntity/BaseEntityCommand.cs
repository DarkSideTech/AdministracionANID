using AUT2Services.Domain.Core.Commands;

namespace AUT2Services.Infra.Security.Accounts.BaseEntity;

public class BaseEntityCommand : Command
{
    public string? CodigoOrganizacion { get; set; }
    public string? NombreOrganizacion { get; set; }
    public Guid? Id_Usuario { get; set; }
    public string? TipoDeEntidad { get; set; }
    public string? CorreoElectronico { get; set; }
    public bool PermitirCorreoElectronicoVacio { get; set; }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new BaseEntityCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}
