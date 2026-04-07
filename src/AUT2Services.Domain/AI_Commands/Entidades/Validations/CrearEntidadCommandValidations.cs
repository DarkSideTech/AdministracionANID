// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.138
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

