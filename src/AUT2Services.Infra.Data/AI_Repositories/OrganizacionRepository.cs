// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.145
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Repositories;

public class OrganizacionRepository : IOrganizacionRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<Organizacion> DbSet;

    public OrganizacionRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<Organizacion>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(Organizacion organizacion)
    {
        DbSet.Add(organizacion); 
    } 

    public void Modificar(Organizacion organizacion)
    {
        DbSet.Update(organizacion); 
    } 

    public void Eliminar(Organizacion organizacion)
    {
        DbSet.Remove(organizacion); 
    } 

    public async Task<IEnumerable<Organizacion>> BuscarTodos() 
    {
        return await DbSet 
                .AsNoTracking() 
                .ToListAsync(); 
    } 

    public async Task<Organizacion> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<Organizacion> BuscarPor_Codigo( 
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

