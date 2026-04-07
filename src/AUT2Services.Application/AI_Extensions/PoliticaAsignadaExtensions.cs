// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.435
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.PoliticasAsignadas;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class PoliticaAsignadaExtensions
{
    public static PoliticaAsignadaViewModel ToViewModel(this PoliticaAsignada politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new PoliticaAsignadaViewModel
        {
            Id = politicaAsignada.Id, 
            Id_Entidad = politicaAsignada.Id_Entidad, 
            Id_Rol = politicaAsignada.Id_Rol, 
            Id_Proceso = politicaAsignada.Id_Proceso, 
            FechaInicioAsignacion = politicaAsignada.FechaInicioAsignacion, 
            FechaTerminoAsignacion = politicaAsignada.FechaTerminoAsignacion, 
            FechaCreacion = politicaAsignada.FechaCreacion, 
            RolRequiereValidacion = politicaAsignada.RolRequiereValidacion, 
            RolAsignadoValidado = politicaAsignada.RolAsignadoValidado, 
            PoliticaAsignadaBase = politicaAsignada.PoliticaAsignadaBase 
        };
    }

    public static IEnumerable<PoliticaAsignadaViewModel> ToViewModel(this IEnumerable<PoliticaAsignada> politicaAsignada)
    {
        return politicaAsignada?.Select(c => c.ToViewModel())!;
    }

    public static PoliticaAsignada ToEntity(this PoliticaAsignadaViewModel politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new PoliticaAsignada(
            (Guid)politicaAsignada.Id!, 
            (Guid)politicaAsignada.Id_Entidad!, 
            (Guid)politicaAsignada.Id_Rol!, 
            (Guid)politicaAsignada.Id_Proceso!, 
            (DateTimeOffset)politicaAsignada.FechaInicioAsignacion!, 
            (DateTimeOffset)politicaAsignada.FechaTerminoAsignacion!, 
            (DateTimeOffset)politicaAsignada.FechaCreacion!, 
            (bool)politicaAsignada.RolRequiereValidacion!, 
            (bool)politicaAsignada.RolAsignadoValidado!, 
            (bool)politicaAsignada.PoliticaAsignadaBase! 
            );
    }


    public static CrearPoliticaAsignadaCommand ToCrearCommand(this CrearPoliticaAsignadaViewModel  politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new CrearPoliticaAsignadaCommand( 
            (Guid)politicaAsignada.Id_Entidad!, 
            (Guid)politicaAsignada.Id_Rol!, 
            (Guid)politicaAsignada.Id_Proceso!, 
            (bool)politicaAsignada.RolRequiereValidacion! 
            );
    }


    public static CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand ToCrearAsignarNuevaEntidadPersonaCommand(this CrearAsignarNuevaEntidadPersonaPoliticaAsignadaViewModel  politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand( 
            (Guid)politicaAsignada.Id_Entidad!, 
            (Guid)politicaAsignada.Id_Rol!, 
            (Guid)politicaAsignada.Id_Proceso! 
            );
    }


    public static EliminarPoliticaAsignadaCommand ToEliminarCommand(this EliminarPoliticaAsignadaViewModel  politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new EliminarPoliticaAsignadaCommand( 
            (Guid)politicaAsignada.Id! 
            );
    }


    public static FinalizaAsignacionPoliticaAsignadaCommand ToFinalizaAsignacionCommand(this FinalizaAsignacionPoliticaAsignadaViewModel  politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new FinalizaAsignacionPoliticaAsignadaCommand( 
            (Guid)politicaAsignada.Id! 
            );
    }


    public static ValidaAsignacionDeRolPoliticaAsignadaCommand ToValidaAsignacionDeRolCommand(this ValidaAsignacionDeRolPoliticaAsignadaViewModel  politicaAsignada)
    {
        if (politicaAsignada is null) return null;

        return new ValidaAsignacionDeRolPoliticaAsignadaCommand( 
            (Guid)politicaAsignada.Id! 
            );
    }

}

