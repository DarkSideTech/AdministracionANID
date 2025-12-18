// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.072
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class Organizacion : Entity, IAggregateRoot
{
    protected Organizacion() { }

    public Organizacion(
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
    }

    public string IdOrganizacion { get; private set; } = string.Empty;
    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public bool OrganizacionBase { get; private set; } = false;
    public bool Activo { get; private set; } = true;

    public void CambiarOrganizacionBase(bool nuevoValor)
    {
        OrganizacionBase = nuevoValor;
    }

    public void CambiarActivo(bool nuevoValor)
    {
        Activo = nuevoValor;
    }
}

