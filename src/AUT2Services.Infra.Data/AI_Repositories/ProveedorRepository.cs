// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.214
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<Proveedor> DbSet;

    public ProveedorRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<Proveedor>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(Proveedor proveedor)
    {
        DbSet.Add(proveedor); 
    } 

    public void Modificar(Proveedor proveedor)
    {
        DbSet.Update(proveedor); 
    } 

    public void Eliminar(Proveedor proveedor)
    {
        DbSet.Remove(proveedor); 
    } 

    public async Task<IEnumerable<Proveedor>> BuscarTodos() 
    {
        return await DbSet 
                .AsNoTracking() 
                .ToListAsync(); 
    } 

    public async Task<Proveedor> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<Proveedor> BuscarPor_Codigo( 
            string codigo 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Codigo.Equals(codigo)
                ); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

