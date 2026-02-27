// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.207
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class Proveedor : Entity, IAggregateRoot
{
    protected Proveedor() { }

    public Proveedor(
        Guid id, 
        string codigo, 
        string nombre, 
        string descripcion, 
        string aPIDeAutenticacion, 
        bool proveedorBase, 
        bool activo 
        )
    {
        Id = id;
        Codigo = codigo;
        Nombre = nombre;
        Descripcion = descripcion;
        APIDeAutenticacion = aPIDeAutenticacion;
        ProveedorBase = proveedorBase;
        Activo = activo;
    }

    public string Codigo { get; private set; } = string.Empty;
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string APIDeAutenticacion { get; private set; } = string.Empty;
    public bool ProveedorBase { get; private set; } = false;
    public bool Activo { get; private set; } = true;

    public void CambiarActivo(bool nuevoValor)
    {
        Activo = nuevoValor;
    }
}

