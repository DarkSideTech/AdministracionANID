// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.113
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

