// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.092
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Validations;

namespace AUT2Services.Domain.Commands.Proveedores.Commands;

public class CrearProveedorCommand : ProveedorCommand
{
    public CrearProveedorCommand(
        string codigo, 
        string nombre, 
        string descripcion, 
        string aPIDeAutenticacion 
        )
    {
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
        APIDeAutenticacion = aPIDeAutenticacion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearProveedorCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

