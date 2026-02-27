// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.228
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Entidades.Events;

public class EntidadEventModificado : Event
{
    public EntidadEventModificado(
        Guid id, 
            string correoElectronico 
        )
    {
        Id = id;
        CorreoElectronico = correoElectronico; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string CorreoElectronico  { get; private set; } = string.Empty; 
}

