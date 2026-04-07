// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.136
// -------------------------------------------------
using AUT2Services.Domain.Commands.Entidades.Validations;

namespace AUT2Services.Domain.Commands.Entidades.Commands;

public class ModificarEntidadCommand : EntidadCommand
{
    public ModificarEntidadCommand(
        Guid id, 
        string correoElectronico 
        )
    {
        Id = id; 
        CorreoElectronico = correoElectronico; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ModificarEntidadCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

