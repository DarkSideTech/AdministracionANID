// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.277
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

