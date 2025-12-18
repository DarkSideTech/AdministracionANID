// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.079
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IPoliticaAsignadaRepository : IRepository<PoliticaAsignada>
{
    void Crear(PoliticaAsignada data);
    void Modificar(PoliticaAsignada data);
    void Eliminar(PoliticaAsignada data);
    Task<PoliticaAsignada> BuscarPor_Id(
        Guid id 
        );
    Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidad_Id_Rol_Id_Proceso(
        Guid id_Entidad, 
        Guid id_Rol, 
        Guid id_Proceso 
        );
    Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidad(
        Guid id_Entidad 
        );
    Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Rol(
        Guid id_Rol 
        );
    Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Proceso(
        Guid id_Proceso 
        );
    Task<IEnumerable<PoliticaAsignada>> BuscarPor_RolRequiereValidacion(
        );
}

