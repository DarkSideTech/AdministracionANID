// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.319
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Repositories;

public class ValidacionEnrrolamientoRepository : IValidacionEnrrolamientoRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<ValidacionEnrrolamiento> DbSet;

    public ValidacionEnrrolamientoRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<ValidacionEnrrolamiento>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(ValidacionEnrrolamiento validacionEnrrolamiento)
    {
        DbSet.Add(validacionEnrrolamiento); 
    } 

    public void Modificar(ValidacionEnrrolamiento validacionEnrrolamiento)
    {
        DbSet.Update(validacionEnrrolamiento); 
    } 

    public void Eliminar(ValidacionEnrrolamiento validacionEnrrolamiento)
    {
        DbSet.Remove(validacionEnrrolamiento); 
    } 

    public async Task<ValidacionEnrrolamiento> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<ValidacionEnrrolamiento> BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario( 
            Guid idValidado_Usuario, 
            Guid idValidaEnrrolamiento_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.IdValidado_Usuario.Equals(idValidado_Usuario)
                && data.IdValidaEnrrolamiento_Usuario.Equals(idValidaEnrrolamiento_Usuario)
                ); 
    } 

    public async Task<IEnumerable<ValidacionEnrrolamiento>> BuscarPor_IdValidado_Usuario( 
            Guid idValidado_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.IdValidado_Usuario.Equals(idValidado_Usuario)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<ValidacionEnrrolamiento>> BuscarPor_IdValidaEnrrolamiento_Usuario( 
            Guid idValidaEnrrolamiento_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.IdValidaEnrrolamiento_Usuario.Equals(idValidaEnrrolamiento_Usuario)
                ) 
                .ToListAsync(); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

