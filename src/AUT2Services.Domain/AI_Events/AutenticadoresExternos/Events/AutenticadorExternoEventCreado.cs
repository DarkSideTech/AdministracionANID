// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.088
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.AutenticadoresExternos.Events;

public class AutenticadorExternoEventCreado : Event
{
    public AutenticadorExternoEventCreado(
        Guid id, 
            Guid id_Proveedor, 
            Guid id_Usuario, 
            string nombreUsuario, 
            string claveDeAcceso, 
            string nombreADesplegar 
        )
    {
        Id = id;
        Id_Proveedor = id_Proveedor; 
        Id_Usuario = id_Usuario; 
        NombreUsuario = nombreUsuario; 
        ClaveDeAcceso = claveDeAcceso; 
        NombreADesplegar = nombreADesplegar; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_Proveedor  { get; private set; } = Guid.Empty; 
    public Guid Id_Usuario  { get; private set; } = Guid.Empty; 
    public string NombreUsuario  { get; private set; } = string.Empty; 
    public string ClaveDeAcceso  { get; private set; } = string.Empty; 
    public string NombreADesplegar  { get; private set; } = string.Empty; 
}

