// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.120
// -------------------------------------------------
using AUT2Services.Domain.Commands.Procesos.Validations;

namespace AUT2Services.Domain.Commands.Procesos.Commands;

public class CrearProcesoCommand : ProcesoCommand
{
    public CrearProcesoCommand(
        Guid idMacro_Proceso, 
        string codigo, 
        string nombre, 
        string descripcion, 
        string contexto, 
        string nivelDeProceso, 
        string url, 
        string token, 
        string comoDesplegarUrlDeProceso, 
        int maximaAsignacionDeRoles 
        )
    {
        IdMacro_Proceso = idMacro_Proceso; 
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
        Contexto = contexto; 
        NivelDeProceso = nivelDeProceso; 
        Url = url; 
        Token = token; 
        ComoDesplegarUrlDeProceso = comoDesplegarUrlDeProceso; 
        MaximaAsignacionDeRoles = maximaAsignacionDeRoles; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearProcesoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

