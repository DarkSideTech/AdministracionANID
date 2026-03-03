// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.923
// -------------------------------------------------
using AUT2Services.Application.ViewModels;
using AUT2Services.Application.ViewModels.ValidacionEnrrolamientos;
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Application.Extensions;

public static class ValidacionEnrrolamientoExtensions
{
    public static ValidacionEnrrolamientoViewModel ToViewModel(this ValidacionEnrrolamiento validacionEnrrolamiento)
    {
        if (validacionEnrrolamiento is null) return null;

        return new ValidacionEnrrolamientoViewModel
        {
            Id = validacionEnrrolamiento.Id, 
            IdValidado_Usuario = validacionEnrrolamiento.IdValidado_Usuario, 
            IdValidaEnrrolamiento_Usuario = validacionEnrrolamiento.IdValidaEnrrolamiento_Usuario, 
            EnrrolamientoAceptado = validacionEnrrolamiento.EnrrolamientoAceptado, 
            FechaValidacion = validacionEnrrolamiento.FechaValidacion, 
            FechaRegistro = validacionEnrrolamiento.FechaRegistro, 
            Activo = validacionEnrrolamiento.Activo 
        };
    }

    public static IEnumerable<ValidacionEnrrolamientoViewModel> ToViewModel(this IEnumerable<ValidacionEnrrolamiento> validacionEnrrolamiento)
    {
        return validacionEnrrolamiento?.Select(c => c.ToViewModel())!;
    }

    public static ValidacionEnrrolamiento ToEntity(this ValidacionEnrrolamientoViewModel validacionEnrrolamiento)
    {
        if (validacionEnrrolamiento is null) return null;

        return new ValidacionEnrrolamiento(
            (Guid)validacionEnrrolamiento.Id!, 
            (Guid)validacionEnrrolamiento.IdValidado_Usuario!, 
            (Guid)validacionEnrrolamiento.IdValidaEnrrolamiento_Usuario!, 
            (bool)validacionEnrrolamiento.EnrrolamientoAceptado!, 
            (DateTimeOffset)validacionEnrrolamiento.FechaValidacion!, 
            (DateTimeOffset)validacionEnrrolamiento.FechaRegistro!, 
            (bool)validacionEnrrolamiento.Activo! 
            );
    }


    public static CrearValidacionEnrrolamientoCommand ToCrearCommand(this CrearValidacionEnrrolamientoViewModel  validacionEnrrolamiento)
    {
        if (validacionEnrrolamiento is null) return null;

        return new CrearValidacionEnrrolamientoCommand( 
            (Guid)validacionEnrrolamiento.IdValidado_Usuario!, 
            (Guid)validacionEnrrolamiento.IdValidaEnrrolamiento_Usuario!, 
            (bool)validacionEnrrolamiento.EnrrolamientoAceptado!, 
            (DateTimeOffset)validacionEnrrolamiento.FechaValidacion! 
            );
    }


    public static EliminarValidacionEnrrolamientoCommand ToEliminarCommand(this EliminarValidacionEnrrolamientoViewModel  validacionEnrrolamiento)
    {
        if (validacionEnrrolamiento is null) return null;

        return new EliminarValidacionEnrrolamientoCommand( 
            (Guid)validacionEnrrolamiento.Id! 
            );
    }


    public static ActivarValidacionEnrrolamientoCommand ToActivarCommand(this ActivarValidacionEnrrolamientoViewModel  validacionEnrrolamiento)
    {
        if (validacionEnrrolamiento is null) return null;

        return new ActivarValidacionEnrrolamientoCommand( 
            (Guid)validacionEnrrolamiento.Id! 
            );
    }


    public static DesactivarValidacionEnrrolamientoCommand ToDesactivarCommand(this DesactivarValidacionEnrrolamientoViewModel  validacionEnrrolamiento)
    {
        if (validacionEnrrolamiento is null) return null;

        return new DesactivarValidacionEnrrolamientoCommand( 
            (Guid)validacionEnrrolamiento.Id! 
            );
    }

}

