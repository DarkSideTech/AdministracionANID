// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.108
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.ServiciosDeDominios.Events;

public class ServicioDeDominioEventEntidadMarcadaComoPrincipal : Event
{
    public ServicioDeDominioEventEntidadMarcadaComoPrincipal(
        Guid id, 
            Guid id_Entidad 
        )
    {
        Id = id;
        Id_Entidad = id_Entidad; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_Entidad  { get; private set; } = Guid.Empty; 
}

