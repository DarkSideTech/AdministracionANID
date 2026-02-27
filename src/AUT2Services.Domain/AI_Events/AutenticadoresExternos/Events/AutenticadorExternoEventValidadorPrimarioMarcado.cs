// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.224
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.AutenticadoresExternos.Events;

public class AutenticadorExternoEventValidadorPrimarioMarcado : Event
{
    public AutenticadorExternoEventValidadorPrimarioMarcado(
        Guid id, 
            bool validadorPrimario 
        )
    {
        Id = id;
        ValidadorPrimario = validadorPrimario; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public bool ValidadorPrimario  { get; private set; } = false; 
}

