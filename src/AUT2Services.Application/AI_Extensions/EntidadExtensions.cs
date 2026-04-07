// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.433
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Entidades;
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class EntidadExtensions
{
    public static EntidadViewModel ToViewModel(this Entidad entidad)
    {
        if (entidad is null) return null;

        return new EntidadViewModel
        {
            Id = entidad.Id, 
            Id_UnidadOrganizacional = entidad.Id_UnidadOrganizacional, 
            Id_Usuario = entidad.Id_Usuario, 
            TipoDeEntidad = entidad.TipoDeEntidad, 
            CorreoElectronico = entidad.CorreoElectronico, 
            FechaInicioAutorizacion = entidad.FechaInicioAutorizacion, 
            FechaTerminoAutorizacion = entidad.FechaTerminoAutorizacion, 
            FechaCreacion = entidad.FechaCreacion, 
            Principal = entidad.Principal, 
            EntidadBase = entidad.EntidadBase 
        };
    }

    public static IEnumerable<EntidadViewModel> ToViewModel(this IEnumerable<Entidad> entidad)
    {
        return entidad?.Select(c => c.ToViewModel())!;
    }

    public static Entidad ToEntity(this EntidadViewModel entidad)
    {
        if (entidad is null) return null;

        return new Entidad(
            (Guid)entidad.Id!, 
            (Guid)entidad.Id_UnidadOrganizacional!, 
            (Guid)entidad.Id_Usuario!, 
            (string)entidad.TipoDeEntidad!, 
            (string)entidad.CorreoElectronico!, 
            (DateTimeOffset)entidad.FechaInicioAutorizacion!, 
            (DateTimeOffset)entidad.FechaTerminoAutorizacion!, 
            (DateTimeOffset)entidad.FechaCreacion!, 
            (bool)entidad.Principal!, 
            (bool)entidad.EntidadBase! 
            );
    }


    public static CrearEntidadCommand ToCrearCommand(this CrearEntidadViewModel  entidad)
    {
        if (entidad is null) return null;

        return new CrearEntidadCommand( 
            (Guid)entidad.Id_UnidadOrganizacional!, 
            (Guid)entidad.Id_Usuario!, 
            (string)entidad.TipoDeEntidad!, 
            (string)entidad.CorreoElectronico! 
            );
    }


    public static ModificarEntidadCommand ToModificarCommand(this ModificarEntidadViewModel  entidad)
    {
        if (entidad is null) return null;

        return new ModificarEntidadCommand( 
            (Guid)entidad.Id!, 
            (string)entidad.CorreoElectronico! 
            );
    }


    public static EliminarEntidadCommand ToEliminarCommand(this EliminarEntidadViewModel  entidad)
    {
        if (entidad is null) return null;

        return new EliminarEntidadCommand( 
            (Guid)entidad.Id! 
            );
    }


    public static FinalizaAutorizacionEntidadCommand ToFinalizaAutorizacionCommand(this FinalizaAutorizacionEntidadViewModel  entidad)
    {
        if (entidad is null) return null;

        return new FinalizaAutorizacionEntidadCommand( 
            (Guid)entidad.Id! 
            );
    }


    public static CambiaEntidadAPrincipalEntidadCommand ToCambiaEntidadAPrincipalCommand(this CambiaEntidadAPrincipalEntidadViewModel  entidad)
    {
        if (entidad is null) return null;

        return new CambiaEntidadAPrincipalEntidadCommand( 
            (Guid)entidad.Id! 
            );
    }


    public static CambiaEntidadANoPrincipalEntidadCommand ToCambiaEntidadANoPrincipalCommand(this CambiaEntidadANoPrincipalEntidadViewModel  entidad)
    {
        if (entidad is null) return null;

        return new CambiaEntidadANoPrincipalEntidadCommand( 
            (Guid)entidad.Id! 
            );
    }

}

