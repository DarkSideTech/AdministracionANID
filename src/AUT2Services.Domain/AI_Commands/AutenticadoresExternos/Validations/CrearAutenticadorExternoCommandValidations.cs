// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.252
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Validations;

public class CrearAutenticadorExternoCommandValidations : AutenticadorExternoValidations<AutenticadorExternoCommand>
{
    public CrearAutenticadorExternoCommandValidations()
    {
        Validate_Id_Proveedor(); 
        Validate_Id_Usuario(); 
        Validate_NombreUsuario(); 
        Validate_ClaveDeAcceso(); 
        Validate_NombreADesplegar(); 
    } 
}

