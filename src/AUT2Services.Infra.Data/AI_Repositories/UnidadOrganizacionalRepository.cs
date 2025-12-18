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

public class UnidadOrganizacionalRepository : IUnidadOrganizacionalRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<UnidadOrganizacional> DbSet;

    public UnidadOrganizacionalRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<UnidadOrganizacional>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(UnidadOrganizacional unidadOrganizacional)
    {
        DbSet.Add(unidadOrganizacional); 
    } 

    public void Modificar(UnidadOrganizacional unidadOrganizacional)
    {
        DbSet.Update(unidadOrganizacional); 
    } 

    public void Eliminar(UnidadOrganizacional unidadOrganizacional)
    {
        DbSet.Remove(unidadOrganizacional); 
    } 

    public async Task<IEnumerable<UnidadOrganizacional>> BuscarTodos() 
    {
        return await DbSet 
                .AsNoTracking() 
                .ToListAsync(); 
    } 

    public async Task<UnidadOrganizacional> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<UnidadOrganizacional> BuscarPor_Codigo_Id_Organizacion( 
            string codigo, 
            Guid id_Organizacion 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Codigo.Equals(codigo)
                && data.Id_Organizacion.Equals(id_Organizacion)
                ); 
    } 

    public async Task<IEnumerable<UnidadOrganizacional>> BuscarPor_Id_Organizacion( 
            Guid id_Organizacion 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Organizacion.Equals(id_Organizacion)
                ) 
                .ToListAsync(); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

