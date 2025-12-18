// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.117
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Validations;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;

public class ValidaAsignacionDeRolPoliticaAsignadaCommand : PoliticaAsignadaCommand
{
    public ValidaAsignacionDeRolPoliticaAsignadaCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ValidaAsignacionDeRolPoliticaAsignadaCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

