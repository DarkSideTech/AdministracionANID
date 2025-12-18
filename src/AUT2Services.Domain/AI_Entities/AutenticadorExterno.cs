// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.072
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class AutenticadorExterno : Entity, IAggregateRoot
{
    protected AutenticadorExterno() { }

    public AutenticadorExterno(
        Guid id, 
        Guid id_Proveedor, 
        Guid id_Usuario, 
        string nombreUsuario, 
        string claveDeAcceso, 
        string nombreADesplegar, 
        bool validadorPrimario, 
        bool autenticadorExternoBase, 
        bool activo 
        )
    {
        Id = id;
        Id_Proveedor = id_Proveedor;
        Id_Usuario = id_Usuario;
        NombreUsuario = nombreUsuario;
        ClaveDeAcceso = claveDeAcceso;
        NombreADesplegar = nombreADesplegar;
        ValidadorPrimario = validadorPrimario;
        AutenticadorExternoBase = autenticadorExternoBase;
        Activo = activo;
    }

    public Guid Id_Proveedor { get; private set; } = Guid.Empty;
    public Guid Id_Usuario { get; private set; } = Guid.Empty;
    public string NombreUsuario { get; private set; } = string.Empty;
    public string ClaveDeAcceso { get; private set; } = string.Empty;
    public string NombreADesplegar { get; private set; } = string.Empty;
    public bool ValidadorPrimario { get; private set; } = false;
    public bool AutenticadorExternoBase { get; private set; } = false;
    public bool Activo { get; private set; } = true;

    public void CambiarValidadorPrimario(bool nuevoValor)
    {
        ValidadorPrimario = nuevoValor;
    }

    public void CambiarActivo(bool nuevoValor)
    {
        Activo = nuevoValor;
    }
}

