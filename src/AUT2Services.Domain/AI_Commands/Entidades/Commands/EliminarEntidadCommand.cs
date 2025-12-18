// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.108
// -------------------------------------------------
using AUT2Services.Domain.Commands.Entidades.Validations;

namespace AUT2Services.Domain.Commands.Entidades.Commands;

public class EliminarEntidadCommand : EntidadCommand
{
    public EliminarEntidadCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarEntidadCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

