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

public class AutenticadorExternoRepository : IAutenticadorExternoRepository
{
    protected readonly AUT2ServicesContext Db;
    protected readonly DbSet<AutenticadorExterno> DbSet;

    public AutenticadorExternoRepository(AUT2ServicesContext contexto)
    {
        Db = contexto;
        DbSet = Db.Set<AutenticadorExterno>();
    }

    public IUnitOfWork UnitOfWork => Db; 

    public void Crear(AutenticadorExterno autenticadorExterno)
    {
        DbSet.Add(autenticadorExterno); 
    } 

    public void Modificar(AutenticadorExterno autenticadorExterno)
    {
        DbSet.Update(autenticadorExterno); 
    } 

    public void Eliminar(AutenticadorExterno autenticadorExterno)
    {
        DbSet.Remove(autenticadorExterno); 
    } 

    public async Task<AutenticadorExterno> BuscarPor_Id( 
            Guid id 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id.Equals(id)
                ); 
    } 

    public async Task<IEnumerable<AutenticadorExterno>> BuscarPor_Id_Proveedor( 
            Guid id_Proveedor 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .Where(data => 
                data.Id_Proveedor.Equals(id_Proveedor)
                ) 
                .ToListAsync(); 
    } 

    public async Task<IEnumerable<AutenticadorExterno>> BuscarPor_Id_Usuario( 
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

    public async Task<AutenticadorExterno> BuscarPor_Id_Usuario_ValidadorPrimario( 
            Guid id_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id_Usuario.Equals(id_Usuario)
                && data.ValidadorPrimario.Equals(true)
                ); 
    } 

    public async Task<AutenticadorExterno> BuscarPor_Id_Proveedor_Id_Usuario( 
            Guid id_Proveedor, 
            Guid id_Usuario 
        ) 
    {
        return await DbSet 
                .AsNoTracking() 
                .FirstOrDefaultAsync(data => 
                data.Id_Proveedor.Equals(id_Proveedor)
                && data.Id_Usuario.Equals(id_Usuario)
                ); 
    } 

    public void Dispose()
    {
        Db.Dispose();
    }  

}

