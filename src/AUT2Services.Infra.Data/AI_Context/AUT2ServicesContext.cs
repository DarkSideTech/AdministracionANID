// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.212
// -------------------------------------------------
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Core.Domain;
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Configurations;
using AUT2Services.Infra.Data.Extensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace AUT2Services.Infra.Data.Context;

public sealed class AUT2ServicesContext : IdentityDbContext<Usuario, Rol, string>, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;

    public AUT2ServicesContext() { }

    public AUT2ServicesContext(DbContextOptions<AUT2ServicesContext> options)
    : base(options)
    {
    }

    public AUT2ServicesContext(
        DbContextOptions<AUT2ServicesContext> options,
        IMediatorHandler mediatorHandler) : base(options)
    {
        _mediatorHandler = mediatorHandler;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }
    public DbSet<Proveedor> Proveedor { get; set; }
    public DbSet<AutenticadorExterno> AutenticadorExterno { get; set; }
    public DbSet<ValidacionEnrrolamiento> ValidacionEnrrolamiento { get; set; }
    public DbSet<UnidadOrganizacional> UnidadOrganizacional { get; set; }
    public DbSet<Entidad> Entidad { get; set; }
    public DbSet<Organizacion> Organizacion { get; set; }
    public DbSet<PoliticaAsignada> PoliticaAsignada { get; set; }
    public DbSet<Proceso> Proceso { get; set; }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        modelBuilder.Ignore<Event>();

        modelBuilder.ApplyConfiguration(new EntityProveedorConfiguration());
        modelBuilder.ApplyConfiguration(new EntityAutenticadorExternoConfiguration());
        modelBuilder.ApplyConfiguration(new EntityValidacionEnrrolamientoConfiguration());
        modelBuilder.ApplyConfiguration(new EntityUnidadOrganizacionalConfiguration());
        modelBuilder.ApplyConfiguration(new EntityEntidadConfiguration());
        modelBuilder.ApplyConfiguration(new EntityOrganizacionConfiguration());
        modelBuilder.ApplyConfiguration(new EntityPoliticaAsignadaConfiguration());
        modelBuilder.ApplyConfiguration(new EntityProcesoConfiguration());

        IncludeBaseData.SeedBaseData(modelBuilder);

        base.OnModelCreating(modelBuilder);
            IncludeBaseData.ConfigureBaseDataSecurity(modelBuilder);
    
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit()
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        var success = await SaveChangesAsync() > 0;

        //this.ChangeTracker.Clear();
        return success;
    }
}

public static class MediatorExtension
{
    public static async Task PublishDomainEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        var tasks = domainEvents
            .Select(async (domainEvent) =>
            {
                await mediator.PublishEvent(domainEvent);
            });

        await Task.WhenAll(tasks);
    }
}

