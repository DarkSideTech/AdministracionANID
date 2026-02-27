// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.226
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;

public class UnidadOrganizacionalEventModificado : Event
{
    public UnidadOrganizacionalEventModificado(
        Guid id, 
            string nombre, 
            string descripcion 
        )
    {
        Id = id;
        Nombre = nombre; 
        Descripcion = descripcion; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
}

