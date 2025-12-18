// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.092
// -------------------------------------------------
using AUT2Services.Domain.Commands.Proveedores.Validations;

namespace AUT2Services.Domain.Commands.Proveedores.Commands;

public class EliminarPor_CodigoProveedorCommand : ProveedorCommand
{
    public EliminarPor_CodigoProveedorCommand(
        string codigo 
        )
    {
        Codigo = codigo; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarPor_CodigoProveedorCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

