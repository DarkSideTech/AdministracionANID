// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.092
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;

public class ValidacionEnrrolamientoEventCreado : Event
{
    public ValidacionEnrrolamientoEventCreado(
        Guid id, 
            Guid idValidado_Usuario, 
            Guid idValidaEnrrolamiento_Usuario, 
            bool enrrolamientoAceptado, 
            DateTimeOffset fechaValidacion, 
            DateTimeOffset fechaRegistro 
        )
    {
        Id = id;
        IdValidado_Usuario = idValidado_Usuario; 
        IdValidaEnrrolamiento_Usuario = idValidaEnrrolamiento_Usuario; 
        EnrrolamientoAceptado = enrrolamientoAceptado; 
        FechaValidacion = fechaValidacion; 
        FechaRegistro = fechaRegistro; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid IdValidado_Usuario  { get; private set; } = Guid.Empty; 
    public Guid IdValidaEnrrolamiento_Usuario  { get; private set; } = Guid.Empty; 
    public bool EnrrolamientoAceptado  { get; private set; } = false; 
    public DateTimeOffset FechaValidacion  { get; private set; } = DateTimeOffset.MinValue; 
    public DateTimeOffset FechaRegistro  { get; private set; } = DateTimeOffset.MinValue; 
}

