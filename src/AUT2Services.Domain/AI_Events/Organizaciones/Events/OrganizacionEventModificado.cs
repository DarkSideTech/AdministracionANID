// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.087
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Organizaciones.Events;

public class OrganizacionEventModificado : Event
{
    public OrganizacionEventModificado(
        Guid id, 
            string idOrganizacion, 
            string nombre, 
            string descripcion 
        )
    {
        Id = id;
        IdOrganizacion = idOrganizacion; 
        Nombre = nombre; 
        Descripcion = descripcion; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string IdOrganizacion  { get; private set; } = string.Empty; 
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
}

