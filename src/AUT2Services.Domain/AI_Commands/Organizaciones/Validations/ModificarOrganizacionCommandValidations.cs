// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.272
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.Organizaciones.Validations;

public class ModificarOrganizacionCommandValidations : OrganizacionValidations<OrganizacionCommand>
{
    public ModificarOrganizacionCommandValidations()
    {
        Validate_Id(); 
        Validate_Nombre(); 
    } 
}

