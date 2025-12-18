// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.116
// -------------------------------------------------
using AUT2Services.Domain.Commands.PoliticasAsignadas.Validations;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;

public class CrearPoliticaAsignadaCommand : PoliticaAsignadaCommand
{
    public CrearPoliticaAsignadaCommand(
        Guid id_Entidad, 
        Guid id_Rol, 
        Guid id_Proceso, 
        bool rolRequiereValidacion 
        )
    {
        Id_Entidad = id_Entidad; 
        Id_Rol = id_Rol; 
        Id_Proceso = id_Proceso; 
        RolRequiereValidacion = rolRequiereValidacion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearPoliticaAsignadaCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

