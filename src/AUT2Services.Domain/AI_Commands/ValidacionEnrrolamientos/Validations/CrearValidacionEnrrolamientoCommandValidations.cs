// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.101
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Validations;

public class CrearValidacionEnrrolamientoCommandValidations : ValidacionEnrrolamientoValidations<ValidacionEnrrolamientoCommand>
{
    public CrearValidacionEnrrolamientoCommandValidations()
    {
        Validate_IdValidado_Usuario(); 
        Validate_IdValidaEnrrolamiento_Usuario(); 
        Validate_EnrrolamientoAceptado(); 
        Validate_FechaValidacion(); 
    } 
}

