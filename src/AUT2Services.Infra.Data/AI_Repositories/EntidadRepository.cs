// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.216
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
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

    public async Task<IEnumerable<Entidad>> BuscarPor_Ids(
            IEnumerable<Guid> ids
        )
    {
        var idsArray = ids.Distinct().ToArray();
        if (idsArray.Length == 0)
        {
            return [];
        }

        return await DbSet
                .AsNoTracking()
                .Where(data => idsArray.Contains(data.Id))
                .ToListAsync();
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

    public async Task<IEnumerable<Entidad>> BuscarPor_Id_Usuario_Id_Organizacion(
            Guid id_Usuario,
            Guid id_Organizacion,
            DateTimeOffset fechaConsulta
        )
    {
        return await (
                from entidad in DbSet.AsNoTracking()
                join unidadOrganizacional in Db.UnidadOrganizacional.AsNoTracking()
                    on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id
                where entidad.Id_Usuario.Equals(id_Usuario)
                    && unidadOrganizacional.Id_Organizacion.Equals(id_Organizacion)
                    && (entidad.FechaInicioAutorizacion == null || entidad.FechaInicioAutorizacion <= fechaConsulta)
                    && (entidad.FechaTerminoAutorizacion == null || entidad.FechaTerminoAutorizacion > fechaConsulta)
                select entidad
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

    public async Task<IEnumerable<Entidad>> BuscarPor_Ids_UnidadOrganizacional(
            IEnumerable<Guid> ids_UnidadOrganizacional
        )
    {
        var ids = ids_UnidadOrganizacional.Distinct().ToArray();
        if (ids.Length == 0)
        {
            return [];
        }

        return await DbSet
                .AsNoTracking()
                .Where(data => ids.Contains(data.Id_UnidadOrganizacional))
                .ToListAsync();
    }

    public async Task<Entidad> BuscarPor_Id_Usuario_TipoDeEntidad_Persona( 
            Guid id_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id_Usuario.Equals(id_Usuario)
                && data.TipoDeEntidad.Equals(EnumTipoDeEntidad.PERSONA)
                && data.Principal.Equals(true)
                ); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

