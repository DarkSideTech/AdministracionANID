// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.925
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.Procesos;
using AUT2Services.Domain.Commands.Procesos.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class ProcesoExtensions
{
    public static ProcesoViewModel ToViewModel(this Proceso proceso)
    {
        if (proceso is null) return null;

        return new ProcesoViewModel
        {
            Id = proceso.Id, 
            IdMacro_Proceso = proceso.IdMacro_Proceso, 
            Codigo = proceso.Codigo, 
            Nombre = proceso.Nombre, 
            Descripcion = proceso.Descripcion, 
            Contexto = proceso.Contexto, 
            NivelDeProceso = proceso.NivelDeProceso, 
            Url = proceso.Url, 
            Token = proceso.Token, 
            ComoDesplegarUrlDeProceso = proceso.ComoDesplegarUrlDeProceso, 
            ProcesoBase = proceso.ProcesoBase, 
            MaximaAsignacionDeRoles = proceso.MaximaAsignacionDeRoles, 
            Activo = proceso.Activo 
        };
    }

    public static IEnumerable<ProcesoViewModel> ToViewModel(this IEnumerable<Proceso> proceso)
    {
        return proceso?.Select(c => c.ToViewModel())!;
    }

    public static Proceso ToEntity(this ProcesoViewModel proceso)
    {
        if (proceso is null) return null;

        return new Proceso(
            (Guid)proceso.Id!, 
            (Guid)proceso.IdMacro_Proceso!, 
            (string)proceso.Codigo!, 
            (string)proceso.Nombre!, 
            (string)proceso.Descripcion!, 
            (string)proceso.Contexto!, 
            (string)proceso.NivelDeProceso!, 
            (string)proceso.Url!, 
            (string)proceso.Token!, 
            (string)proceso.ComoDesplegarUrlDeProceso!, 
            (bool)proceso.ProcesoBase!, 
            (int)proceso.MaximaAsignacionDeRoles!, 
            (bool)proceso.Activo! 
            );
    }


    public static CrearProcesoCommand ToCrearCommand(this CrearProcesoViewModel  proceso)
    {
        if (proceso is null) return null;

        return new CrearProcesoCommand( 
            (Guid)proceso.IdMacro_Proceso!, 
            (string)proceso.Codigo!, 
            (string)proceso.Nombre!, 
            (string)proceso.Descripcion!, 
            (string)proceso.Contexto!, 
            (string)proceso.NivelDeProceso!, 
            (string)proceso.Url!, 
            (string)proceso.Token!, 
            (string)proceso.ComoDesplegarUrlDeProceso!, 
            (int)proceso.MaximaAsignacionDeRoles! 
            );
    }


    public static ModificarProcesoCommand ToModificarCommand(this ModificarProcesoViewModel  proceso)
    {
        if (proceso is null) return null;

        return new ModificarProcesoCommand( 
            (Guid)proceso.Id!, 
            (string)proceso.Nombre!, 
            (string)proceso.Descripcion!, 
            (string)proceso.Contexto!, 
            (string)proceso.NivelDeProceso!, 
            (string)proceso.Url!, 
            (string)proceso.Token!, 
            (string)proceso.ComoDesplegarUrlDeProceso!, 
            (int)proceso.MaximaAsignacionDeRoles! 
            );
    }


    public static EliminarProcesoCommand ToEliminarCommand(this EliminarProcesoViewModel  proceso)
    {
        if (proceso is null) return null;

        return new EliminarProcesoCommand( 
            (Guid)proceso.Id! 
            );
    }


    public static ActivarProcesoCommand ToActivarCommand(this ActivarProcesoViewModel  proceso)
    {
        if (proceso is null) return null;

        return new ActivarProcesoCommand( 
            (Guid)proceso.Id! 
            );
    }


    public static DesactivarProcesoCommand ToDesactivarCommand(this DesactivarProcesoViewModel  proceso)
    {
        if (proceso is null) return null;

        return new DesactivarProcesoCommand( 
            (Guid)proceso.Id! 
            );
    }

}

