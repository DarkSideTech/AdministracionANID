// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.112
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Validations;

namespace AUT2Services.Domain.Commands.Organizaciones.Commands;

public class ModificarOrganizacionCommand : OrganizacionCommand
{
    public ModificarOrganizacionCommand(
        Guid id, 
        string idOrganizacion, 
        string nombre, 
        string descripcion 
        )
    {
        Id = id; 
        IdOrganizacion = idOrganizacion; 
        Nombre = nombre; 
        Descripcion = descripcion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new ModificarOrganizacionCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

