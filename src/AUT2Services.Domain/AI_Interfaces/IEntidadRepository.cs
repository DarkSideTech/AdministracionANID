// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.076
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IEntidadRepository : IRepository<Entidad>
{
    void Crear(Entidad data);
    void Modificar(Entidad data);
    void Eliminar(Entidad data);
    Task<Entidad> BuscarPor_Id(
        Guid id 
        );
    Task<IEnumerable<Entidad>> BuscarPor_Ids(
        IEnumerable<Guid> ids
        );
    Task<Entidad> BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal(
        Guid id_Usuario, 
        Guid id_UnidadOrganizacional 
        );
    Task<Entidad> BuscarPor_Id_Usuario_Id_UnidadOrganizacional(
        Guid id_Usuario, 
        Guid id_UnidadOrganizacional 
        );
    Task<IEnumerable<Entidad>> BuscarPor_Id_Usuario(
        Guid id_Usuario 
        );
    Task<IEnumerable<Entidad>> BuscarPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario,
        Guid id_Organizacion,
        DateTimeOffset fechaConsulta
        );
    Task<IEnumerable<Entidad>> BuscarPor_Id_UnidadOrganizacional(
        Guid id_UnidadOrganizacional 
        );
    Task<IEnumerable<Entidad>> BuscarPor_Ids_UnidadOrganizacional(
        IEnumerable<Guid> ids_UnidadOrganizacional
        );
    Task<Entidad> BuscarPor_Id_Usuario_TipoDeEntidad_Persona(
        Guid id_Usuario 
        );
}

