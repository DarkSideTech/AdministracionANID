// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.121
// -------------------------------------------------
using AUT2Services.Domain.Commands.Procesos.Validations;

namespace AUT2Services.Domain.Commands.Procesos.Commands;

public class EliminarProcesoCommand : ProcesoCommand
{
    public EliminarProcesoCommand(
        Guid id 
        )
    {
        Id = id; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarProcesoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

