// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.260
// -------------------------------------------------
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Validations;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;

public class EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand : UnidadOrganizacionalCommand
{
    public EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand(
        string codigo, 
        Guid id_Organizacion 
        )
    {
        Codigo = codigo; 
        Id_Organizacion = id_Organizacion; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

