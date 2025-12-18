// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.078
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IProveedorRepository : IRepository<Proveedor>
{
    void Crear(Proveedor data);
    void Modificar(Proveedor data);
    void Eliminar(Proveedor data);
    Task<IEnumerable<Proveedor>> BuscarTodos(
        );
    Task<Proveedor> BuscarPor_Id(
        Guid id 
        );
    Task<Proveedor> BuscarPor_Codigo(
        string codigo 
        );
}

