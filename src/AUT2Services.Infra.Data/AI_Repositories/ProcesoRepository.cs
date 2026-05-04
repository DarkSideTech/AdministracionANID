// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.217
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Repositories;

public class ProcesoRepository : IProcesoRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<Proceso> DbSet;

    public ProcesoRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<Proceso>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(Proceso proceso)
    {
        DbSet.Add(proceso); 
    } 

    public void Modificar(Proceso proceso)
    {
        DbSet.Update(proceso); 
    } 

    public void Eliminar(Proceso proceso)
    {
        DbSet.Remove(proceso); 
    } 

    public async Task<IEnumerable<Proceso>> BuscarTodos() 
    {
        return await DbSet 
                .AsNoTracking() 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<ProcesoSeleccionDTO>> BuscarActivosParaSeleccion()
    {
        return await DbSet
                .AsNoTracking()
                .Where(data => data.Activo)
                .OrderBy(data => data.Codigo)
                .ThenBy(data => data.Nombre)
                .Select(data => new ProcesoSeleccionDTO
                {
                    Id = data.Id,
                    Codigo = data.Codigo,
                    Nombre = data.Nombre
                })
                .ToListAsync();
    }

    public async Task<Proceso> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<Proceso> BuscarPor_Codigo( 
            string codigo 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Codigo.Equals(codigo)
                ); 
    } 

    public async Task<IEnumerable<Proceso>> BuscarPor_IdMacro_Proceso( 
            Guid idMacro_Proceso 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.IdMacro_Proceso.Equals(idMacro_Proceso)
                ) 
                .ToListAsync(); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

