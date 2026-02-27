// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.276
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Validations;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;

public class EliminarPoliticaAsignadaCommand : PoliticaAsignadaCommand
{
    public EliminarPoliticaAsignadaCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarPoliticaAsignadaCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

