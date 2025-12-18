// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.101
// -------------------------------------------------
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Validations;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;

public class CrearValidacionEnrrolamientoCommand : ValidacionEnrrolamientoCommand
{
    public CrearValidacionEnrrolamientoCommand(
        Guid idValidado_Usuario, 
        Guid idValidaEnrrolamiento_Usuario, 
        bool enrrolamientoAceptado, 
        DateTimeOffset fechaValidacion 
        )
    {
        IdValidado_Usuario = idValidado_Usuario; 
        IdValidaEnrrolamiento_Usuario = idValidaEnrrolamiento_Usuario; 
        EnrrolamientoAceptado = enrrolamientoAceptado; 
        FechaValidacion = fechaValidacion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearValidacionEnrrolamientoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

