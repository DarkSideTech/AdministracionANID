// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.117
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Validations;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;

public class FinalizaAsignacionPoliticaAsignadaCommand : PoliticaAsignadaCommand
{
    public FinalizaAsignacionPoliticaAsignadaCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new FinalizaAsignacionPoliticaAsignadaCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

