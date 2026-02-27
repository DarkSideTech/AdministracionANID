// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.307
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Proveedores;
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class ProveedorExtensions
{
    public static ProveedorViewModel ToViewModel(this Proveedor proveedor)
    {
        if (proveedor is null) return null;

        return new ProveedorViewModel
        {
            Id = proveedor.Id, 
            Codigo = proveedor.Codigo, 
            Nombre = proveedor.Nombre, 
            Descripcion = proveedor.Descripcion, 
            APIDeAutenticacion = proveedor.APIDeAutenticacion, 
            ProveedorBase = proveedor.ProveedorBase, 
            Activo = proveedor.Activo 
        };
    }

    public static IEnumerable<ProveedorViewModel> ToViewModel(this IEnumerable<Proveedor> proveedor)
    {
        return proveedor?.Select(c => c.ToViewModel())!;
    }

    public static Proveedor ToEntity(this ProveedorViewModel proveedor)
    {
        if (proveedor is null) return null;

        return new Proveedor(
            (Guid)proveedor.Id!, 
            (string)proveedor.Codigo!, 
            (string)proveedor.Nombre!, 
            (string)proveedor.Descripcion!, 
            (string)proveedor.APIDeAutenticacion!, 
            (bool)proveedor.ProveedorBase!, 
            (bool)proveedor.Activo! 
            );
    }


    public static CrearProveedorCommand ToCrearCommand(this CrearProveedorViewModel  proveedor)
    {
        if (proveedor is null) return null;

        return new CrearProveedorCommand( 
            (string)proveedor.Codigo!, 
            (string)proveedor.Nombre!, 
            (string)proveedor.Descripcion!, 
            (string)proveedor.APIDeAutenticacion! 
            );
    }


    public static ModificarProveedorCommand ToModificarCommand(this ModificarProveedorViewModel  proveedor)
    {
        if (proveedor is null) return null;

        return new ModificarProveedorCommand( 
            (Guid)proveedor.Id!, 
            (string)proveedor.Nombre!, 
            (string)proveedor.Descripcion!, 
            (string)proveedor.APIDeAutenticacion! 
            );
    }


    public static EliminarProveedorCommand ToEliminarCommand(this EliminarProveedorViewModel  proveedor)
    {
        if (proveedor is null) return null;

        return new EliminarProveedorCommand( 
            (Guid)proveedor.Id! 
            );
    }


    public static EliminarPor_CodigoProveedorCommand ToEliminarPor_CodigoCommand(this EliminarPor_CodigoProveedorViewModel  proveedor)
    {
        if (proveedor is null) return null;

        return new EliminarPor_CodigoProveedorCommand( 
            (string)proveedor.Codigo! 
            );
    }


    public static ActivarProveedorCommand ToActivarCommand(this ActivarProveedorViewModel  proveedor)
    {
        if (proveedor is null) return null;

        return new ActivarProveedorCommand( 
            (Guid)proveedor.Id! 
            );
    }


    public static DesactivarProveedorCommand ToDesactivarCommand(this DesactivarProveedorViewModel  proveedor)
    {
        if (proveedor is null) return null;

        return new DesactivarProveedorCommand( 
            (Guid)proveedor.Id! 
            );
    }

}

