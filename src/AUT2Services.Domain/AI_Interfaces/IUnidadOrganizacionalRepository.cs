// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.075
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IUnidadOrganizacionalRepository : IRepository<UnidadOrganizacional>
{
    void Crear(UnidadOrganizacional data);
    void Modificar(UnidadOrganizacional data);
    void Eliminar(UnidadOrganizacional data);
    Task<IEnumerable<UnidadOrganizacional>> BuscarTodos(
        );
    Task<UnidadOrganizacional> BuscarPor_Id(
        Guid id 
        );
    Task<UnidadOrganizacional> BuscarPor_Codigo_Id_Organizacion(
        string codigo, 
        Guid id_Organizacion 
        );
    Task<IEnumerable<UnidadOrganizacional>> BuscarPor_Id_Organizacion(
        Guid id_Organizacion 
        );
}

