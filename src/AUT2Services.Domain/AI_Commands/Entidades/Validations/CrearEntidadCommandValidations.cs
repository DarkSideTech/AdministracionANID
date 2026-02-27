// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.266
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.Entidades.Validations;

public class CrearEntidadCommandValidations : EntidadValidations<EntidadCommand>
{
    public CrearEntidadCommandValidations()
    {
        Validate_Id_UnidadOrganizacional(); 
        Validate_Id_Usuario(); 
        Validate_TipoDeEntidad(); 
        Validate_CorreoElectronico(); 
    } 
}

