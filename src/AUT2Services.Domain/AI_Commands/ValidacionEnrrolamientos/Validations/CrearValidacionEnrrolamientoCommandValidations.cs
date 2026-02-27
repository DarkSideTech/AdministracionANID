// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.256
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

