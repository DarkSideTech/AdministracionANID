// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.320
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Repositories;

public class EntidadRepository : IEntidadRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<Entidad> DbSet;

    public EntidadRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<Entidad>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(Entidad entidad)
    {
        DbSet.Add(entidad); 
    } 

    public void Modificar(Entidad entidad)
    {
        DbSet.Update(entidad); 
    } 

    public void Eliminar(Entidad entidad)
    {
        DbSet.Remove(entidad); 
    } 

    public async Task<Entidad> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<Entidad> BuscarPor_Id_Usuario_Id_UnidadOrganizacional_Principal( 
            Guid id_Usuario, 
            Guid id_UnidadOrganizacional 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id_Usuario.Equals(id_Usuario)
                && data.Id_UnidadOrganizacional.Equals(id_UnidadOrganizacional)
	            && data.Principal.Equals(true)
                ); 
    } 

    public async Task<Entidad> BuscarPor_Id_Usuario_Id_UnidadOrganizacional( 
            Guid id_Usuario, 
            Guid id_UnidadOrganizacional 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id_Usuario.Equals(id_Usuario)
                && data.Id_UnidadOrganizacional.Equals(id_UnidadOrganizacional)
                ); 
    } 

    public async Task<IEnumerable<Entidad>> BuscarPor_Id_Usuario( 
            Guid id_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Usuario.Equals(id_Usuario)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<Entidad>> BuscarPor_Id_UnidadOrganizacional( 
            Guid id_UnidadOrganizacional 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_UnidadOrganizacional.Equals(id_UnidadOrganizacional)
                ) 
                .ToListAsync(); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

