// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.104
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Validations;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;

public class DesactivarUnidadOrganizacionalCommand : UnidadOrganizacionalCommand
{
    public DesactivarUnidadOrganizacionalCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new DesactivarUnidadOrganizacionalCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

