// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.122
// -------------------------------------------------
using AUT2Services.Domain.Commands.AutenticadoresExternos.Validations;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;

public class CrearAutenticadorExternoCommand : AutenticadorExternoCommand
{
    public CrearAutenticadorExternoCommand(
        Guid id_Proveedor, 
        Guid id_Usuario, 
        string nombreUsuario, 
        string claveDeAcceso, 
        string nombreADesplegar 
        )
    {
        Id_Proveedor = id_Proveedor; 
        Id_Usuario = id_Usuario; 
        NombreUsuario = nombreUsuario; 
        ClaveDeAcceso = claveDeAcceso; 
        NombreADesplegar = nombreADesplegar; 
    }

    public override bool IsValid()
    {
        CommandResponse.ValidationResult = new CrearAutenticadorExternoCommandValidations().Validate(this);
        return CommandResponse.ValidationResult.IsValid;
    }
}

