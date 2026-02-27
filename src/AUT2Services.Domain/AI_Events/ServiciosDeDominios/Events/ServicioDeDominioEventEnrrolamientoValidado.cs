// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.234
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.ServiciosDeDominios.Events;

public class ServicioDeDominioEventEnrrolamientoValidado : Event
{
    public ServicioDeDominioEventEnrrolamientoValidado(
        Guid id, 
            Guid idValidado_Usuario, 
            Guid idValidaEnrrolamiento_Usuario 
        )
    {
        Id = id;
        IdValidado_Usuario = idValidado_Usuario; 
        IdValidaEnrrolamiento_Usuario = idValidaEnrrolamiento_Usuario; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid IdValidado_Usuario  { get; private set; } = Guid.Empty; 
    public Guid IdValidaEnrrolamiento_Usuario  { get; private set; } = Guid.Empty; 
}

