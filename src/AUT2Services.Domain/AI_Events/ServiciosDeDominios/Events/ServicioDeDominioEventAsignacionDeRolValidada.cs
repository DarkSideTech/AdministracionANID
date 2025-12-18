// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.090
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.ServiciosDeDominios.Events;

public class ServicioDeDominioEventAsignacionDeRolValidada : Event
{
    public ServicioDeDominioEventAsignacionDeRolValidada(
        Guid id, 
            Guid id_PoliticaAsignada, 
            Guid id_Usuario_ValidaAsignacionDeRol 
        )
    {
        Id = id;
        Id_PoliticaAsignada = id_PoliticaAsignada; 
        Id_Usuario_ValidaAsignacionDeRol = id_Usuario_ValidaAsignacionDeRol; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_PoliticaAsignada  { get; private set; } = Guid.Empty; 
    public Guid Id_Usuario_ValidaAsignacionDeRol  { get; private set; } = Guid.Empty; 
}

