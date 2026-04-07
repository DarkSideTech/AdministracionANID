// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.433
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.UnidadesOrganizacionales;
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class UnidadOrganizacionalExtensions
{
    public static UnidadOrganizacionalViewModel ToViewModel(this UnidadOrganizacional unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new UnidadOrganizacionalViewModel
        {
            Id = unidadOrganizacional.Id, 
            Id_Organizacion = unidadOrganizacional.Id_Organizacion, 
            Codigo = unidadOrganizacional.Codigo, 
            Nombre = unidadOrganizacional.Nombre, 
            Descripcion = unidadOrganizacional.Descripcion, 
            UnidadOrganizacionalBase = unidadOrganizacional.UnidadOrganizacionalBase, 
            Activo = unidadOrganizacional.Activo 
        };
    }

    public static IEnumerable<UnidadOrganizacionalViewModel> ToViewModel(this IEnumerable<UnidadOrganizacional> unidadOrganizacional)
    {
        return unidadOrganizacional?.Select(c => c.ToViewModel())!;
    }

    public static UnidadOrganizacional ToEntity(this UnidadOrganizacionalViewModel unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new UnidadOrganizacional(
            (Guid)unidadOrganizacional.Id!, 
            (Guid)unidadOrganizacional.Id_Organizacion!, 
            (string)unidadOrganizacional.Codigo!, 
            (string)unidadOrganizacional.Nombre!, 
            (string)unidadOrganizacional.Descripcion!, 
            (bool)unidadOrganizacional.UnidadOrganizacionalBase!, 
            (bool)unidadOrganizacional.Activo! 
            );
    }


    public static CrearUnidadOrganizacionalCommand ToCrearCommand(this CrearUnidadOrganizacionalViewModel  unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new CrearUnidadOrganizacionalCommand( 
            (Guid)unidadOrganizacional.Id_Organizacion!, 
            (string)unidadOrganizacional.Codigo!, 
            (string)unidadOrganizacional.Nombre!, 
            (string)unidadOrganizacional.Descripcion! 
            );
    }


    public static ModificarUnidadOrganizacionalCommand ToModificarCommand(this ModificarUnidadOrganizacionalViewModel  unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new ModificarUnidadOrganizacionalCommand( 
            (Guid)unidadOrganizacional.Id!, 
            (string)unidadOrganizacional.Nombre!, 
            (string)unidadOrganizacional.Descripcion! 
            );
    }


    public static EliminarUnidadOrganizacionalCommand ToEliminarCommand(this EliminarUnidadOrganizacionalViewModel  unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new EliminarUnidadOrganizacionalCommand( 
            (Guid)unidadOrganizacional.Id! 
            );
    }


    public static EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand ToEliminarPor_Codigo_Id_OrganizacionCommand(this EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalViewModel  unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand( 
            (string)unidadOrganizacional.Codigo!, 
            (Guid)unidadOrganizacional.Id_Organizacion! 
            );
    }


    public static ActivarUnidadOrganizacionalCommand ToActivarCommand(this ActivarUnidadOrganizacionalViewModel  unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new ActivarUnidadOrganizacionalCommand( 
            (Guid)unidadOrganizacional.Id! 
            );
    }


    public static DesactivarUnidadOrganizacionalCommand ToDesactivarCommand(this DesactivarUnidadOrganizacionalViewModel  unidadOrganizacional)
    {
        if (unidadOrganizacional is null) return null;

        return new DesactivarUnidadOrganizacionalCommand( 
            (Guid)unidadOrganizacional.Id! 
            );
    }

}

