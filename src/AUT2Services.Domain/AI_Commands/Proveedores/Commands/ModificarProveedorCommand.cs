// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.092
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

