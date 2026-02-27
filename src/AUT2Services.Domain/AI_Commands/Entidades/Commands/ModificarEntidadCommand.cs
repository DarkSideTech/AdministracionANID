// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.265
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

