// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.121
// -------------------------------------------------
namespace AUT2Services.Domain.Commands.Procesos.Validations;

public class ModificarProcesoCommandValidations : ProcesoValidations<ProcesoCommand>
{
    public ModificarProcesoCommandValidations()
    {
        Validate_Id(); 
        Validate_Nombre(); 
        Validate_NivelDeProceso(); 
        Validate_Url(); 
        Validate_Token(); 
        Validate_ComoDesplegarUrlDeProceso(); 
        Validate_MaximaAsignacionDeRoles(); 
    } 
}

