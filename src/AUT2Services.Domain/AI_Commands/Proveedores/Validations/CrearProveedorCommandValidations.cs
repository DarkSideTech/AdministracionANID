// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.114
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.Proveedores.Validations;

public class CrearProveedorCommandValidations : ProveedorValidations<ProveedorCommand>
{
    public CrearProveedorCommandValidations()
    {
        Validate_Codigo(); 
        Validate_Nombre(); 
        Validate_Descripcion(); 
        Validate_APIDeAutenticacion(); 
    } 
}

