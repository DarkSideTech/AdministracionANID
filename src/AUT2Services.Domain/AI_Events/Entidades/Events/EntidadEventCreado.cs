// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.084
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.Entidades.Events;

public class EntidadEventCreado : Event
{
    public EntidadEventCreado(
        Guid id, 
            Guid id_UnidadOrganizacional, 
            Guid id_Usuario, 
            string tipoDeEntidad, 
            string correoElectronico, 
            bool principal, 
            bool entidadBase 
        )
    {
        Id = id;
        Id_UnidadOrganizacional = id_UnidadOrganizacional; 
        Id_Usuario = id_Usuario; 
        TipoDeEntidad = tipoDeEntidad; 
        CorreoElectronico = correoElectronico; 
        Principal = principal; 
        EntidadBase = entidadBase; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_UnidadOrganizacional  { get; private set; } = Guid.Empty; 
    public Guid Id_Usuario  { get; private set; } = Guid.Empty; 
    public string TipoDeEntidad  { get; private set; } = string.Empty; 
    public string CorreoElectronico  { get; private set; } = string.Empty; 
    public bool Principal  { get; private set; } = false; 
    public bool EntidadBase  { get; private set; } = false; 
}

