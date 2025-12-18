// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.109
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

