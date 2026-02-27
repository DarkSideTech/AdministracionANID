// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.250
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Validations;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;

public class EliminarAutenticadorExternoCommand : AutenticadorExternoCommand
{
    public EliminarAutenticadorExternoCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarAutenticadorExternoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

