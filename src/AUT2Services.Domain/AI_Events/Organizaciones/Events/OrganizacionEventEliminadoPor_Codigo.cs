// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.101
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Organizaciones.Events;

public class OrganizacionEventEliminadoPor_Codigo : Event
{
    public OrganizacionEventEliminadoPor_Codigo(
        Guid id, 
            string codigo 
        )
    {
        Id = id;
        Codigo = codigo; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string Codigo  { get; private set; } = string.Empty; 
}

