// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.104
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Validations;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;

public class CrearUnidadOrganizacionalCommand : UnidadOrganizacionalCommand
{
    public CrearUnidadOrganizacionalCommand(
        Guid id_Organizacion, 
        string codigo, 
        string nombre, 
        string descripcion 
        )
    {
        Id_Organizacion = id_Organizacion; 
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearUnidadOrganizacionalCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

