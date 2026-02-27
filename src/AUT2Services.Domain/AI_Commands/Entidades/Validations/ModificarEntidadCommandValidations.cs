// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.266
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.Entidades.Validations;

public class ModificarEntidadCommandValidations : EntidadValidations<EntidadCommand>
{
    public ModificarEntidadCommandValidations()
    {
        Validate_Id(); 
        Validate_CorreoElectronico(); 
    } 
}

