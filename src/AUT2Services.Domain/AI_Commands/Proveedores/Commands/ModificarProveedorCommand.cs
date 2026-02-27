// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.239
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Validations;

namespace AUT2Services.Domain.Commands.Proveedores.Commands;

public class ModificarProveedorCommand : ProveedorCommand
{
    public ModificarProveedorCommand(
        Guid id, 
        string nombre, 
        string descripcion, 
        string aPIDeAutenticacion 
        )
    {
        Id = id; 
        Nombre = nombre; 
        Descripcion = descripcion; 
        APIDeAutenticacion = aPIDeAutenticacion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ModificarProveedorCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

