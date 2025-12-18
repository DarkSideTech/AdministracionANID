// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.096
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Validations;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;

public class DesactivarAutenticadorExternoCommand : AutenticadorExternoCommand
{
    public DesactivarAutenticadorExternoCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new DesactivarAutenticadorExternoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

