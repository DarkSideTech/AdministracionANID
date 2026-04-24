// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.212
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Core.Domain;
using AUT2Services.Domain.Core.Events;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Configurations;
using AUT2Services.Infra.Data.Extensions;
using AUT2Services.Infra.DataTrazabilidad.Persistence;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace AUT2Services.Infra.Data.Context;

public sealed class AUT2ServicesContext : IdentityDbContext<Usuario, Rol, string>, IUnitOfWork
{
    private const int MaxAuditCommitRetries = 5;
    private static readonly ConcurrentDictionary<Guid, SemaphoreSlim> AuditAggregateLocks = new();
    private readonly IMediatorHandler? _mediatorHandler;
    private readonly IAuditBuffer? _auditBuffer;
    private readonly IAuditExecutionContext? _auditExecutionContext;
    private readonly IAuditDeltaBuilder? _auditDeltaBuilder;
    private readonly IAuditSerializer? _auditSerializer;
    private readonly IClock? _clock;
    private readonly List<Event> _deferredDomainEvents = [];

    public AUT2ServicesContext() { }

    public AUT2ServicesContext(DbContextOptions<AUT2ServicesContext> options)
        : base(options)
    {
    }

    public AUT2ServicesContext(
        DbContextOptions<AUT2ServicesContext> options,
        IMediatorHandler mediatorHandler,
        IAuditBuffer auditBuffer,
        IAuditExecutionContext auditExecutionContext,
        IAuditDeltaBuilder auditDeltaBuilder,
        IAuditSerializer auditSerializer,
        IClock clock) : base(options)
    {
        _mediatorHandler = mediatorHandler;
        _auditBuffer = auditBuffer;
        _auditExecutionContext = auditExecutionContext;
        _auditDeltaBuilder = auditDeltaBuilder;
        _auditSerializer = auditSerializer;
        _clock = clock;
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
    public DbSet<AuditOutboxMessage> AuditOutboxMessages => Set<AuditOutboxMessage>();
    public DbSet<AuditOutboxChange> AuditOutboxChanges => Set<AuditOutboxChange>();
    public DbSet<AuditAggregateCursor> AuditAggregateCursors => Set<AuditAggregateCursor>();
    public DbSet<NotificationOutboxMessage> NotificationOutboxMessages => Set<NotificationOutboxMessage>();
    public DbSet<PasswordChangeChallenge> PasswordChangeChallenges => Set<PasswordChangeChallenge>();

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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditOutboxMessageConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit()
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .OrderBy(x => x.Timestamp)
            .ToList();

        var pendingAuditEntries = _auditBuffer?.Drain() ?? [];
        var auditAggregateIds = pendingAuditEntries
            .Select(entry => entry.DomainEvent.AggregateId)
            .Where(id => id != Guid.Empty)
            .Distinct()
            .OrderBy(id => id)
            .ToArray();

        var heldAuditLocks = await AcquireAuditAggregateLocksAsync(auditAggregateIds, CancellationToken.None).ConfigureAwait(false);
        try
        {
            var currentTransaction = Database.CurrentTransaction;
            return currentTransaction is null
                ? await CommitWithoutExternalTransactionAsync(domainEntities, domainEvents, pendingAuditEntries).ConfigureAwait(false)
                : await CommitWithinExternalTransactionAsync(currentTransaction, domainEntities, domainEvents, pendingAuditEntries).ConfigureAwait(false);
        }
        finally
        {
            ReleaseAuditAggregateLocks(heldAuditLocks);
        }
    }

    public async Task CommitExternalTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        await FlushDeferredDomainEventsAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task RollbackExternalTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
    {
        try
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            DiscardDeferredDomainEvents();
        }
    }

    public async Task FlushDeferredDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        if (_mediatorHandler is null || _deferredDomainEvents.Count == 0)
        {
            return;
        }

        foreach (var deferredDomainEvent in _deferredDomainEvents
                     .OrderBy(x => x.Timestamp)
                     .ThenBy(x => x.AggregateId)
                     .ThenBy(x => x.AggregateRevision)
                     .ToList())
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _mediatorHandler.PublishEvent(deferredDomainEvent).ConfigureAwait(false);
            _deferredDomainEvents.Remove(deferredDomainEvent);
        }
    }

    public void DiscardDeferredDomainEvents()
    {
        _deferredDomainEvents.Clear();
    }

    private async Task<bool> CommitWithoutExternalTransactionAsync(
        IReadOnlyCollection<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity>> domainEntities,
        IReadOnlyCollection<Event> domainEvents,
        IReadOnlyList<PendingAuditEntry> pendingAuditEntries)
    {
        var maxAttempts = pendingAuditEntries.Count == 0 ? 1 : MaxAuditCommitRetries;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                if (pendingAuditEntries.Count != 0)
                {
                    await PrepareAuditOutboxMessagesAsync(pendingAuditEntries).ConfigureAwait(false);
                }

                var success = await SaveChangesAsync().ConfigureAwait(false) > 0;
                if (!success)
                {
                    return false;
                }

                ClearDomainEvents(domainEntities);

                if (_mediatorHandler is not null && domainEvents.Count != 0)
                {
                    await PublishDomainEventsSequentiallyAsync(domainEvents, CancellationToken.None).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex) when (attempt < maxAttempts && IsRetryableAuditRevisionConflict(ex))
            {
                ResetTrackedAuditPersistenceState();
                await Task.Delay(GetAuditRetryDelay(attempt), CancellationToken.None).ConfigureAwait(false);
            }
        }

        return false;
    }

    private async Task<bool> CommitWithinExternalTransactionAsync(
        IDbContextTransaction transaction,
        IReadOnlyCollection<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity>> domainEntities,
        IReadOnlyCollection<Event> domainEvents,
        IReadOnlyList<PendingAuditEntry> pendingAuditEntries)
    {
        var canRetry = pendingAuditEntries.Count != 0 && transaction.SupportsSavepoints;
        var maxAttempts = canRetry ? MaxAuditCommitRetries : 1;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var savepointName = canRetry ? CreateAuditSavepointName() : null;
            if (savepointName is not null)
            {
                await transaction.CreateSavepointAsync(savepointName, CancellationToken.None).ConfigureAwait(false);
            }

            try
            {
                if (pendingAuditEntries.Count != 0)
                {
                    await PrepareAuditOutboxMessagesAsync(pendingAuditEntries).ConfigureAwait(false);
                }

                var success = await SaveChangesAsync().ConfigureAwait(false) > 0;
                if (!success)
                {
                    return false;
                }

                ClearDomainEvents(domainEntities);

                if (domainEvents.Count != 0)
                {
                    _deferredDomainEvents.AddRange(domainEvents);
                }

                if (savepointName is not null)
                {
                    await transaction.ReleaseSavepointAsync(savepointName, CancellationToken.None).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex) when (attempt < maxAttempts && IsRetryableAuditRevisionConflict(ex))
            {
                if (savepointName is not null)
                {
                    await transaction.RollbackToSavepointAsync(savepointName, CancellationToken.None).ConfigureAwait(false);
                }

                ResetTrackedAuditPersistenceState();
                await Task.Delay(GetAuditRetryDelay(attempt), CancellationToken.None).ConfigureAwait(false);
            }
        }

        return false;
    }

    private async Task PrepareAuditOutboxMessagesAsync(IReadOnlyList<PendingAuditEntry> pendingAuditEntries)
    {
        if (_auditDeltaBuilder is null || _auditSerializer is null)
        {
            return;
        }

        var aggregateIds = pendingAuditEntries
            .Select(entry => entry.DomainEvent.AggregateId)
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        var existingCursors = aggregateIds.Count == 0
            ? new Dictionary<Guid, AuditAggregateCursor>()
            : await AuditAggregateCursors
                .AsTracking()
                .Where(cursor => aggregateIds.Contains(cursor.AggregateId))
                .ToDictionaryAsync(cursor => cursor.AggregateId)
                .ConfigureAwait(false);

        var persistedAtUtc = _clock?.UtcNow ?? DateTimeOffset.UtcNow;

        foreach (var auditGroup in pendingAuditEntries
                     .Where(entry => entry.DomainEvent.AggregateId != Guid.Empty)
                     .GroupBy(entry => entry.DomainEvent.AggregateId)
                     .OrderBy(group => group.Min(entry => entry.CaptureOrder)))
        {
            if (!existingCursors.TryGetValue(auditGroup.Key, out var cursor))
            {
                cursor = new AuditAggregateCursor
                {
                    AggregateId = auditGroup.Key,
                    LastRevision = 0,
                    UpdatedAtUtc = persistedAtUtc,
                    ConcurrencyToken = Guid.NewGuid()
                };

                existingCursors.Add(cursor.AggregateId, cursor);
                AuditAggregateCursors.Add(cursor);
            }

            var revision = cursor.LastRevision;

            foreach (var pendingAuditEntry in auditGroup.OrderBy(entry => entry.CaptureOrder))
            {
                revision++;
                pendingAuditEntry.DomainEvent.SetAggregateRevision(revision);

                var auditOutboxMessage = BuildAuditOutboxMessage(pendingAuditEntry, revision, persistedAtUtc);
                AuditOutboxMessages.Add(auditOutboxMessage);
            }

            cursor.LastRevision = revision;
            cursor.UpdatedAtUtc = persistedAtUtc;
            cursor.ConcurrencyToken = Guid.NewGuid();

            if (Entry(cursor).State == EntityState.Unchanged)
            {
                Entry(cursor).State = EntityState.Modified;
            }
        }
    }

    private AuditOutboxMessage BuildAuditOutboxMessage(
        PendingAuditEntry pendingAuditEntry,
        long aggregateRevision,
        DateTimeOffset persistedAtUtc)
    {
        var auditOutboxMessage = new AuditOutboxMessage
        {
            Id = Guid.NewGuid(),
            CorrelationId = _auditExecutionContext?.CorrelationId ?? Guid.NewGuid(),
            AggregateId = pendingAuditEntry.DomainEvent.AggregateId,
            AggregateType = pendingAuditEntry.AggregateType,
            AggregateRevision = aggregateRevision,
            EventType = pendingAuditEntry.DomainEvent.MessageType,
            CommandType = pendingAuditEntry.CommandType,
            OperationType = pendingAuditEntry.OperationType,
            ActorUserId = _auditExecutionContext?.UserId,
            ActorUsername = _auditExecutionContext?.Username,
            ActorEmail = _auditExecutionContext?.Email,
            RequestPath = _auditExecutionContext?.RequestPath,
            OccurredAtUtc = pendingAuditEntry.DomainEvent.Timestamp,
            PersistedAtUtc = persistedAtUtc,
            SnapshotJson = pendingAuditEntry.OperationType == AuditOperationType.Create
                           || (pendingAuditEntry.OperationType == AuditOperationType.Update && pendingAuditEntry.IncludeSnapshot)
                ? _auditSerializer!.Serialize(pendingAuditEntry.After)
                : null,
            DispatchStatus = 0,
            DispatchAttempts = 0,
            SchemaVersion = 1
        };

        if (pendingAuditEntry.OperationType == AuditOperationType.Update)
        {
            var changes = _auditDeltaBuilder!.Build(pendingAuditEntry.Before, pendingAuditEntry.After);
            foreach (var change in changes)
            {
                auditOutboxMessage.Changes.Add(new AuditOutboxChange
                {
                    Id = Guid.NewGuid(),
                    AuditOutboxMessageId = auditOutboxMessage.Id,
                    Order = change.Order,
                    Path = change.Path,
                    ValueType = change.ValueType,
                    NewValueJson = change.NewValueJson
                });
            }
        }

        return auditOutboxMessage;
    }

    private static async Task<Stack<SemaphoreSlim>> AcquireAuditAggregateLocksAsync(
        IReadOnlyCollection<Guid> auditAggregateIds,
        CancellationToken cancellationToken)
    {
        var heldAuditLocks = new Stack<SemaphoreSlim>(auditAggregateIds.Count);

        foreach (var aggregateId in auditAggregateIds)
        {
            var auditLock = AuditAggregateLocks.GetOrAdd(aggregateId, static _ => new SemaphoreSlim(1, 1));
            await auditLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            heldAuditLocks.Push(auditLock);
        }

        return heldAuditLocks;
    }

    private static void ReleaseAuditAggregateLocks(Stack<SemaphoreSlim> heldAuditLocks)
    {
        while (heldAuditLocks.Count != 0)
        {
            heldAuditLocks.Pop().Release();
        }
    }

    private static void ClearDomainEvents(IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Entity>> domainEntities)
    {
        foreach (var domainEntity in domainEntities)
        {
            domainEntity.Entity.ClearDomainEvents();
        }
    }

    private void ResetTrackedAuditPersistenceState()
    {
        foreach (var entry in ChangeTracker.Entries<AuditOutboxChange>().ToList())
        {
            entry.State = EntityState.Detached;
        }

        foreach (var entry in ChangeTracker.Entries<AuditOutboxMessage>().ToList())
        {
            entry.State = EntityState.Detached;
        }

        foreach (var entry in ChangeTracker.Entries<AuditAggregateCursor>().ToList())
        {
            entry.State = EntityState.Detached;
        }
    }

    private static string CreateAuditSavepointName()
        => $"audit_{Guid.NewGuid():N}";

    private static TimeSpan GetAuditRetryDelay(int attempt)
    {
        var baseDelayMilliseconds = attempt switch
        {
            1 => 25,
            2 => 50,
            3 => 100,
            4 => 150,
            _ => 250
        };

        return TimeSpan.FromMilliseconds(baseDelayMilliseconds + Random.Shared.Next(5, 25));
    }

    private static bool IsRetryableAuditRevisionConflict(Exception exception)
    {
        if (exception is DbUpdateConcurrencyException)
        {
            return true;
        }

        return IsRetryableProviderAuditConflict(exception);
    }

    private static bool IsRetryableProviderAuditConflict(Exception exception)
    {
        var exceptionTypeName = exception.GetType().FullName;

        if (string.Equals(exceptionTypeName, "Npgsql.PostgresException", StringComparison.Ordinal))
        {
            return IsRetryablePostgresAuditConflict(exception);
        }

        if (string.Equals(exceptionTypeName, "Microsoft.Data.SqlClient.SqlException", StringComparison.Ordinal))
        {
            return IsRetryableSqlServerAuditConflict(exception);
        }

        return exception.InnerException is not null && IsRetryableProviderAuditConflict(exception.InnerException);
    }

    private static bool IsRetryablePostgresAuditConflict(Exception exception)
    {
        var sqlState = GetPropertyValue<string>(exception, "SqlState");
        if (!string.Equals(sqlState, "23505", StringComparison.Ordinal))
        {
            return false;
        }

        var constraintName = GetPropertyValue<string>(exception, "ConstraintName");
        if (IsRetryableAuditConstraint(constraintName))
        {
            return true;
        }

        var message = exception.Message;
        return ContainsRetryableAuditObjectName(message);
    }

    private static bool IsRetryableSqlServerAuditConflict(Exception exception)
    {
        if (GetPropertyValue<IEnumerable>(exception, "Errors") is not IEnumerable errors)
        {
            return ContainsRetryableAuditObjectName(exception.Message);
        }

        foreach (var error in errors)
        {
            var errorNumber = GetPropertyValue<int?>(error, "Number");
            if (errorNumber is not 2601 and not 2627)
            {
                continue;
            }

            var message = GetPropertyValue<string>(error, "Message") ?? exception.Message;
            if (ContainsRetryableAuditObjectName(message))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsRetryableAuditConstraint(string? constraintName)
        => string.Equals(constraintName, "IX_AuditOutbox_AggregateId_AggregateRevision", StringComparison.OrdinalIgnoreCase)
           || string.Equals(constraintName, "PK_AuditAggregateCursor", StringComparison.OrdinalIgnoreCase);

    private static bool ContainsRetryableAuditObjectName(string? message)
        => !string.IsNullOrWhiteSpace(message)
           && (message.Contains("IX_AuditOutbox_AggregateId_AggregateRevision", StringComparison.OrdinalIgnoreCase)
               || message.Contains("PK_AuditAggregateCursor", StringComparison.OrdinalIgnoreCase)
               || message.Contains("AuditOutbox", StringComparison.OrdinalIgnoreCase)
               || message.Contains("AuditAggregateCursor", StringComparison.OrdinalIgnoreCase));

    private static T? GetPropertyValue<T>(object source, string propertyName)
    {
        var property = source.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (property is null)
        {
            return default;
        }

        var value = property.GetValue(source);
        if (value is null)
        {
            return default;
        }

        if (value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    private async Task PublishDomainEventsSequentiallyAsync(
        IEnumerable<Event> domainEvents,
        CancellationToken cancellationToken)
    {
        if (_mediatorHandler is null)
        {
            return;
        }

        foreach (var domainEvent in domainEvents
                     .OrderBy(x => x.Timestamp)
                     .ThenBy(x => x.AggregateId)
                     .ThenBy(x => x.AggregateRevision))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _mediatorHandler.PublishEvent(domainEvent).ConfigureAwait(false);
        }
    }
}
