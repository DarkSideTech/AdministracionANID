// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.117
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Validations;

public class CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommandValidations : PoliticaAsignadaValidations<PoliticaAsignadaCommand>
{
    public CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommandValidations()
    {
        Validate_Id_Entidad(); 
        Validate_Id_Rol(); 
        Validate_Id_Proceso(); 
    } 
}

