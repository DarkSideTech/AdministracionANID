// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.283
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

