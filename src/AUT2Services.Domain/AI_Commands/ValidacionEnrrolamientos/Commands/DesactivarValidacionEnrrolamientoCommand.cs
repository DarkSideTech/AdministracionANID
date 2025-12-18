// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.101
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Validations;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;

public class DesactivarValidacionEnrrolamientoCommand : ValidacionEnrrolamientoCommand
{
    public DesactivarValidacionEnrrolamientoCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new DesactivarValidacionEnrrolamientoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

