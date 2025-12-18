// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.079
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IProcesoRepository : IRepository<Proceso>
{
    void Crear(Proceso data);
    void Modificar(Proceso data);
    void Eliminar(Proceso data);
    Task<IEnumerable<Proceso>> BuscarTodos(
        );
    Task<Proceso> BuscarPor_Id(
        Guid id 
        );
    Task<Proceso> BuscarPor_Codigo(
        string codigo 
        );
    Task<IEnumerable<Proceso>> BuscarPor_IdMacro_Proceso(
        Guid idMacro_Proceso 
        );
}

