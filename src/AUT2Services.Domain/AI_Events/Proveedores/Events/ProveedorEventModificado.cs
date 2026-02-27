// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.222
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Proveedores.Events;

public class ProveedorEventModificado : Event
{
    public ProveedorEventModificado(
        Guid id, 
            string nombre, 
            string descripcion, 
            string aPIDeAutenticacion 
        )
    {
        Id = id;
        Nombre = nombre; 
        Descripcion = descripcion; 
        APIDeAutenticacion = aPIDeAutenticacion; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
    public string APIDeAutenticacion  { get; private set; } = string.Empty; 
}

