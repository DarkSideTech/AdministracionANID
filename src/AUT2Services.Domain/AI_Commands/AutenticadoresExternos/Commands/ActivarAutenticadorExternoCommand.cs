// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.123
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Validations;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;

public class ActivarAutenticadorExternoCommand : AutenticadorExternoCommand
{
    public ActivarAutenticadorExternoCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ActivarAutenticadorExternoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

