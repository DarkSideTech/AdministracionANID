// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.087
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Organizaciones.Events;

public class OrganizacionEventCreado : Event
{
    public OrganizacionEventCreado(
        Guid id, 
            string idOrganizacion, 
            string codigo, 
            string nombre, 
            string descripcion, 
            bool organizacionBase, 
            bool activo 
        )
    {
        Id = id;
        IdOrganizacion = idOrganizacion; 
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
        OrganizacionBase = organizacionBase; 
        Activo = activo; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string IdOrganizacion  { get; private set; } = string.Empty; 
    public string Codigo  { get; private set; } = string.Empty; 
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
    public bool OrganizacionBase  { get; private set; } = false; 
    public bool Activo  { get; private set; } = true; 
}

