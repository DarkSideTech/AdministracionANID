// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.209
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class Entidad : Entity, IAggregateRoot
{
    protected Entidad() { }

    public Entidad(
        Guid id, 
        Guid id_UnidadOrganizacional, 
        Guid id_Usuario, 
        string tipoDeEntidad, 
        string correoElectronico, 
        DateTimeOffset fechaInicioAutorizacion, 
        DateTimeOffset fechaTerminoAutorizacion, 
        DateTimeOffset fechaCreacion, 
        bool principal, 
        bool entidadBase 
        )
    {
        Id = id;
        Id_UnidadOrganizacional = id_UnidadOrganizacional;
        Id_Usuario = id_Usuario;
        TipoDeEntidad = tipoDeEntidad;
        CorreoElectronico = correoElectronico;
        FechaInicioAutorizacion = fechaInicioAutorizacion;
        FechaTerminoAutorizacion = fechaTerminoAutorizacion;
        FechaCreacion = fechaCreacion;
        Principal = principal;
        EntidadBase = entidadBase;
    }

    public Guid Id_UnidadOrganizacional { get; private set; } = Guid.Empty;
    public Guid Id_Usuario { get; private set; } = Guid.Empty;
    public string TipoDeEntidad { get; private set; } = string.Empty;
    public string CorreoElectronico { get; private set; } = string.Empty;
    public DateTimeOffset FechaInicioAutorizacion { get; private set; } = DateTimeOffset.MinValue;
    public DateTimeOffset FechaTerminoAutorizacion { get; private set; } = DateTimeOffset.MinValue;
    public DateTimeOffset FechaCreacion { get; private set; } = DateTimeOffset.MinValue;
    public bool Principal { get; private set; } = false;
    public bool EntidadBase { get; private set; } = true;

    public void CambiarFechaInicioAutorizacion(DateTimeOffset nuevoValor)
    {
        FechaInicioAutorizacion = nuevoValor;
    }

    public void CambiarFechaTerminoAutorizacion(DateTimeOffset nuevoValor)
    {
        FechaTerminoAutorizacion = nuevoValor;
    }

    public void CambiarFechaCreacion(DateTimeOffset nuevoValor)
    {
        FechaCreacion = nuevoValor;
    }

    public void CambiarPrincipal(bool nuevoValor)
    {
        Principal = nuevoValor;
    }

    public void CambiarEntidadBase(bool nuevoValor)
    {
        EntidadBase = nuevoValor;
    }
}

