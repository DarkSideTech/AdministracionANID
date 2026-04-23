// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.039
// -------------------------------------------------
using AUT2Services.Domain.Core.Domain;

namespace AUT2Services.Domain.Entities;

public class ValidacionEnrrolamiento : Entity, IAggregateRoot
{
    protected ValidacionEnrrolamiento() { }

    public ValidacionEnrrolamiento(
        Guid id, 
        Guid idValidado_Usuario, 
        Guid idValidaEnrrolamiento_Usuario, 
        bool enrrolamientoAceptado, 
        DateTimeOffset? fechaValidacion, 
        DateTimeOffset? fechaRegistro, 
        bool activo 
        )
    {
        Id = id;
        IdValidado_Usuario = idValidado_Usuario;
        IdValidaEnrrolamiento_Usuario = idValidaEnrrolamiento_Usuario;
        EnrrolamientoAceptado = enrrolamientoAceptado;
        FechaValidacion = fechaValidacion;
        FechaRegistro = fechaRegistro;
        Activo = activo;
    }

    public Guid IdValidado_Usuario { get; private set; } = Guid.Empty;
    public Guid IdValidaEnrrolamiento_Usuario { get; private set; } = Guid.Empty;
    public bool EnrrolamientoAceptado { get; private set; } = false;
    public DateTimeOffset? FechaValidacion { get; private set; }
    public DateTimeOffset? FechaRegistro { get; private set; }
    public bool Activo { get; private set; } = true;

    public void CambiarEnrrolamientoAceptado(bool nuevoValor)
    {
        EnrrolamientoAceptado = nuevoValor;
    }

    public void CambiarFechaValidacion(DateTimeOffset? nuevoValor)
    {
        FechaValidacion = nuevoValor;
    }

    public void CambiarFechaRegistro(DateTimeOffset? nuevoValor)
    {
        FechaRegistro = nuevoValor;
    }

    public void CambiarActivo(bool nuevoValor)
    {
        Activo = nuevoValor;
    }
}

