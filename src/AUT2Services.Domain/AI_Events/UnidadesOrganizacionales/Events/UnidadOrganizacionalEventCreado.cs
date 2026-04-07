// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.094
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;

public class UnidadOrganizacionalEventCreado : Event
{
    public UnidadOrganizacionalEventCreado(
        Guid id, 
            Guid id_Organizacion, 
            string codigo, 
            string nombre, 
            string descripcion, 
            bool unidadOrganizacionalBase, 
            bool activo 
        )
    {
        Id = id;
        Id_Organizacion = id_Organizacion; 
        Codigo = codigo; 
        Nombre = nombre; 
        Descripcion = descripcion; 
        UnidadOrganizacionalBase = unidadOrganizacionalBase; 
        Activo = activo; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_Organizacion  { get; private set; } = Guid.Empty; 
    public string Codigo  { get; private set; } = string.Empty; 
    public string Nombre  { get; private set; } = string.Empty; 
    public string Descripcion  { get; private set; } = string.Empty; 
    public bool UnidadOrganizacionalBase  { get; private set; } = false; 
    public bool Activo  { get; private set; } = true; 
}

