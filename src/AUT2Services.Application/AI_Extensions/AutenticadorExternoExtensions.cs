// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.431
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.AutenticadoresExternos;
using AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class AutenticadorExternoExtensions
{
    public static AutenticadorExternoViewModel ToViewModel(this AutenticadorExterno autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new AutenticadorExternoViewModel
        {
            Id = autenticadorExterno.Id, 
            Id_Proveedor = autenticadorExterno.Id_Proveedor, 
            Id_Usuario = autenticadorExterno.Id_Usuario, 
            NombreUsuario = autenticadorExterno.NombreUsuario, 
            ClaveDeAcceso = autenticadorExterno.ClaveDeAcceso, 
            NombreADesplegar = autenticadorExterno.NombreADesplegar, 
            ValidadorPrimario = autenticadorExterno.ValidadorPrimario, 
            AutenticadorExternoBase = autenticadorExterno.AutenticadorExternoBase, 
            Activo = autenticadorExterno.Activo 
        };
    }

    public static IEnumerable<AutenticadorExternoViewModel> ToViewModel(this IEnumerable<AutenticadorExterno> autenticadorExterno)
    {
        return autenticadorExterno?.Select(c => c.ToViewModel())!;
    }

    public static AutenticadorExterno ToEntity(this AutenticadorExternoViewModel autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new AutenticadorExterno(
            (Guid)autenticadorExterno.Id!, 
            (Guid)autenticadorExterno.Id_Proveedor!, 
            (Guid)autenticadorExterno.Id_Usuario!, 
            (string)autenticadorExterno.NombreUsuario!, 
            (string)autenticadorExterno.ClaveDeAcceso!, 
            (string)autenticadorExterno.NombreADesplegar!, 
            (bool)autenticadorExterno.ValidadorPrimario!, 
            (bool)autenticadorExterno.AutenticadorExternoBase!, 
            (bool)autenticadorExterno.Activo! 
            );
    }


    public static CrearAutenticadorExternoCommand ToCrearCommand(this CrearAutenticadorExternoViewModel  autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new CrearAutenticadorExternoCommand( 
            (Guid)autenticadorExterno.Id_Proveedor!, 
            (Guid)autenticadorExterno.Id_Usuario!, 
            (string)autenticadorExterno.NombreUsuario!, 
            (string)autenticadorExterno.ClaveDeAcceso!, 
            (string)autenticadorExterno.NombreADesplegar! 
            );
    }


    public static ModificarAutenticadorExternoCommand ToModificarCommand(this ModificarAutenticadorExternoViewModel  autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new ModificarAutenticadorExternoCommand( 
            (Guid)autenticadorExterno.Id!, 
            (string)autenticadorExterno.ClaveDeAcceso!, 
            (string)autenticadorExterno.NombreADesplegar! 
            );
    }


    public static EliminarAutenticadorExternoCommand ToEliminarCommand(this EliminarAutenticadorExternoViewModel  autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new EliminarAutenticadorExternoCommand( 
            (Guid)autenticadorExterno.Id! 
            );
    }


    public static MarcarComoValidadorPrimarioAutenticadorExternoCommand ToMarcarComoValidadorPrimarioCommand(this MarcarComoValidadorPrimarioAutenticadorExternoViewModel  autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new MarcarComoValidadorPrimarioAutenticadorExternoCommand( 
            (Guid)autenticadorExterno.Id! 
            );
    }


    public static ActivarAutenticadorExternoCommand ToActivarCommand(this ActivarAutenticadorExternoViewModel  autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new ActivarAutenticadorExternoCommand( 
            (Guid)autenticadorExterno.Id! 
            );
    }


    public static DesactivarAutenticadorExternoCommand ToDesactivarCommand(this DesactivarAutenticadorExternoViewModel  autenticadorExterno)
    {
        if (autenticadorExterno is null) return null;

        return new DesactivarAutenticadorExternoCommand( 
            (Guid)autenticadorExterno.Id! 
            );
    }

}

