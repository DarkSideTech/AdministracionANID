// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.124
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Validations;

public class ModificarAutenticadorExternoCommandValidations : AutenticadorExternoValidations<AutenticadorExternoCommand>
{
    public ModificarAutenticadorExternoCommandValidations()
    {
        Validate_Id(); 
        Validate_ClaveDeAcceso(); 
        Validate_NombreADesplegar(); 
    } 
}

