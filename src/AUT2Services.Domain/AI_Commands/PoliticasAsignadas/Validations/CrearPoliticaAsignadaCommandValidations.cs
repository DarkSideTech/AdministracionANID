// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.149
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Validations;

public class CrearPoliticaAsignadaCommandValidations : PoliticaAsignadaValidations<PoliticaAsignadaCommand>
{
    public CrearPoliticaAsignadaCommandValidations()
    {
        Validate_Id_Entidad(); 
        Validate_Id_Rol(); 
        Validate_Id_Proceso(); 
        Validate_RolRequiereValidacion(); 
    } 
}

