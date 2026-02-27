// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.256
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Validations;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;

public class EliminarValidacionEnrrolamientoCommand : ValidacionEnrrolamientoCommand
{
    public EliminarValidacionEnrrolamientoCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarValidacionEnrrolamientoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

