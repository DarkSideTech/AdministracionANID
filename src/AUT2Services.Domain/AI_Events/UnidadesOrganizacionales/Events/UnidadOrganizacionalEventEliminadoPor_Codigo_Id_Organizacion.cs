// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.226
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;

public class UnidadOrganizacionalEventEliminadoPor_Codigo_Id_Organizacion : Event
{
    public UnidadOrganizacionalEventEliminadoPor_Codigo_Id_Organizacion(
        Guid id, 
            string codigo, 
            Guid id_Organizacion 
        )
    {
        Id = id;
        Codigo = codigo; 
        Id_Organizacion = id_Organizacion; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string Codigo  { get; private set; } = string.Empty; 
    public Guid Id_Organizacion  { get; private set; } = Guid.Empty; 
}

