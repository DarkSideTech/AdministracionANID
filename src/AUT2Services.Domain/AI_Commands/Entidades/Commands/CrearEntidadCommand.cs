// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.264
// -------------------------------------------------
using AUT2Services.Domain.Commands.Entidades.Validations;

namespace AUT2Services.Domain.Commands.Entidades.Commands;

public class CrearEntidadCommand : EntidadCommand
{
    public CrearEntidadCommand(
        Guid id_UnidadOrganizacional, 
        Guid id_Usuario, 
        string tipoDeEntidad, 
        string correoElectronico 
        )
    {
        Id_UnidadOrganizacional = id_UnidadOrganizacional; 
        Id_Usuario = id_Usuario; 
        TipoDeEntidad = tipoDeEntidad; 
        CorreoElectronico = correoElectronico; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearEntidadCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

