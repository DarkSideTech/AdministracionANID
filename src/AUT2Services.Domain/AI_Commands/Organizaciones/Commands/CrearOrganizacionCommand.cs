// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.142
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Validations;

namespace AUT2Services.Domain.Commands.Organizaciones.Commands;

public class CrearOrganizacionCommand : OrganizacionCommand
{
    public CrearOrganizacionCommand(
        string idOrganizacion, 
        string codigo, 
        string nombre, 
        string descripcion 
        )
    {
        IdOrganizacion = idOrganizacion; 
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearOrganizacionCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

