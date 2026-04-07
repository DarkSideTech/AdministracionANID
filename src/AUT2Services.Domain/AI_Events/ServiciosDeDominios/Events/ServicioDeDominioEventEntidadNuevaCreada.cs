// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.107
// -------------------------------------------------
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.DTOs;

namespace AUT2Services.Domain.Events.ServiciosDeDominios.Events;

public class ServicioDeDominioEventEntidadNuevaCreada : Event
{
    public ServicioDeDominioEventEntidadNuevaCreada(
        Guid id, 
            Guid id_UnidadOrganizacional, 
            Guid id_Usuario, 
            string tipoDeEntidad, 
            string correoElectronico 
        )
    {
        Id = id;
        Id_UnidadOrganizacional = id_UnidadOrganizacional; 
        Id_Usuario = id_Usuario; 
        TipoDeEntidad = tipoDeEntidad; 
        CorreoElectronico = correoElectronico; 

        AggregateId = id;
    }

    public Guid Id { get; private set; } = Guid.Empty;
    public Guid Id_UnidadOrganizacional  { get; private set; } = Guid.Empty; 
    public Guid Id_Usuario  { get; private set; } = Guid.Empty; 
    public string TipoDeEntidad  { get; private set; } = string.Empty; 
    public string CorreoElectronico  { get; private set; } = string.Empty; 
}

