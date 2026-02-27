// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.311
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Organizaciones;
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class OrganizacionExtensions
{
    public static OrganizacionViewModel ToViewModel(this Organizacion organizacion)
    {
        if (organizacion is null) return null;

        return new OrganizacionViewModel
        {
            Id = organizacion.Id, 
            IdOrganizacion = organizacion.IdOrganizacion, 
            Codigo = organizacion.Codigo, 
            Nombre = organizacion.Nombre, 
            Descripcion = organizacion.Descripcion, 
            OrganizacionBase = organizacion.OrganizacionBase, 
            Activo = organizacion.Activo 
        };
    }

    public static IEnumerable<OrganizacionViewModel> ToViewModel(this IEnumerable<Organizacion> organizacion)
    {
        return organizacion?.Select(c => c.ToViewModel())!;
    }

    public static Organizacion ToEntity(this OrganizacionViewModel organizacion)
    {
        if (organizacion is null) return null;

        return new Organizacion(
            (Guid)organizacion.Id!, 
            (string)organizacion.IdOrganizacion!, 
            (string)organizacion.Codigo!, 
            (string)organizacion.Nombre!, 
            (string)organizacion.Descripcion!, 
            (bool)organizacion.OrganizacionBase!, 
            (bool)organizacion.Activo! 
            );
    }


    public static CrearOrganizacionCommand ToCrearCommand(this CrearOrganizacionViewModel  organizacion)
    {
        if (organizacion is null) return null;

        return new CrearOrganizacionCommand( 
            (string)organizacion.IdOrganizacion!, 
            (string)organizacion.Codigo!, 
            (string)organizacion.Nombre!, 
            (string)organizacion.Descripcion! 
            );
    }


    public static ModificarOrganizacionCommand ToModificarCommand(this ModificarOrganizacionViewModel  organizacion)
    {
        if (organizacion is null) return null;

        return new ModificarOrganizacionCommand( 
            (Guid)organizacion.Id!, 
            (string)organizacion.IdOrganizacion!, 
            (string)organizacion.Nombre!, 
            (string)organizacion.Descripcion! 
            );
    }


    public static EliminarOrganizacionCommand ToEliminarCommand(this EliminarOrganizacionViewModel  organizacion)
    {
        if (organizacion is null) return null;

        return new EliminarOrganizacionCommand( 
            (Guid)organizacion.Id! 
            );
    }


    public static EliminarPor_CodigoOrganizacionCommand ToEliminarPor_CodigoCommand(this EliminarPor_CodigoOrganizacionViewModel  organizacion)
    {
        if (organizacion is null) return null;

        return new EliminarPor_CodigoOrganizacionCommand( 
            (string)organizacion.Codigo! 
            );
    }


    public static ActivarOrganizacionCommand ToActivarCommand(this ActivarOrganizacionViewModel  organizacion)
    {
        if (organizacion is null) return null;

        return new ActivarOrganizacionCommand( 
            (Guid)organizacion.Id! 
            );
    }


    public static DesactivarOrganizacionCommand ToDesactivarCommand(this DesactivarOrganizacionViewModel  organizacion)
    {
        if (organizacion is null) return null;

        return new DesactivarOrganizacionCommand( 
            (Guid)organizacion.Id! 
            );
    }

}

