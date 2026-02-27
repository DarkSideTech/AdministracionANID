// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.271
// -------------------------------------------------
using AUT2Services.Domain.Commands.Organizaciones.Validations;

namespace AUT2Services.Domain.Commands.Organizaciones.Commands;

public class EliminarPor_CodigoOrganizacionCommand : OrganizacionCommand
{
    public EliminarPor_CodigoOrganizacionCommand(
        string codigo 
        )
    {
        Codigo = codigo; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new EliminarPor_CodigoOrganizacionCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

