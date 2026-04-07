// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.217
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Repositories;

public class PoliticaAsignadaRepository : IPoliticaAsignadaRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<PoliticaAsignada> DbSet;

    public PoliticaAsignadaRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<PoliticaAsignada>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(PoliticaAsignada politicaAsignada)
    {
        DbSet.Add(politicaAsignada); 
    } 

    public void Modificar(PoliticaAsignada politicaAsignada)
    {
        DbSet.Update(politicaAsignada); 
    } 

    public void Eliminar(PoliticaAsignada politicaAsignada)
    {
        DbSet.Remove(politicaAsignada); 
    } 

    public async Task<PoliticaAsignada> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidad_Id_Rol_Id_Proceso( 
            Guid id_Entidad, 
            Guid id_Rol, 
            Guid id_Proceso 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Entidad.Equals(id_Entidad)
                && data.Id_Rol.Equals(id_Rol)
                && data.Id_Proceso.Equals(id_Proceso)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidad( 
            Guid id_Entidad 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Entidad.Equals(id_Entidad)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Rol( 
            Guid id_Rol 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Rol.Equals(id_Rol)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Proceso( 
            Guid id_Proceso 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Proceso.Equals(id_Proceso)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<PoliticaAsignada>> BuscarPor_RolRequiereValidacion() 
    {
        return await DbSet 
                .AsNoTracking() 
                .ToListAsync(); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

