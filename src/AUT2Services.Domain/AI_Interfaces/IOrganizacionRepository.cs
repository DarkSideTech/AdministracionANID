// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.216
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IOrganizacionRepository : IRepository<Organizacion>
{
    void Crear(Organizacion data);
    void Modificar(Organizacion data);
    void Eliminar(Organizacion data);
    Task<IEnumerable<Organizacion>> BuscarTodos(
        );
    Task<Organizacion> BuscarPor_Id(
        Guid id 
        );
    Task<Organizacion> BuscarPor_Codigo(
        string codigo 
        );
}

