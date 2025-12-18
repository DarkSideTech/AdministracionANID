// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.082
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.AutenticadoresExternos.Events;

public class AutenticadorExternoEventModificado : Event
{
    public AutenticadorExternoEventModificado(
        Guid id, 
            string claveDeAcceso, 
            string nombreADesplegar 
        )
    {
        Id = id;
        ClaveDeAcceso = claveDeAcceso; 
        NombreADesplegar = nombreADesplegar; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string ClaveDeAcceso  { get; private set; } = string.Empty; 
    public string NombreADesplegar  { get; private set; } = string.Empty; 
}

