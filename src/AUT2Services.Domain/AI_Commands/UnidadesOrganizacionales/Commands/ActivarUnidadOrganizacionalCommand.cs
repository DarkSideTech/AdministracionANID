// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.260
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Validations;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;

public class ActivarUnidadOrganizacionalCommand : UnidadOrganizacionalCommand
{
    public ActivarUnidadOrganizacionalCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ActivarUnidadOrganizacionalCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

