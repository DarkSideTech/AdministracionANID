using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class AuditOutboxMessageConfiguration : IEntityTypeConfiguration<AuditOutboxMessage>
{
    public void Configure(EntityTypeBuilder<AuditOutboxMessage> builder)
    {
        builder.ToTable("AuditOutbox");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.AggregateType).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.EventType).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.CommandType).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.ActorUserId).HasMaxLength(100);
        builder.Property(entity => entity.ActorUsername).HasMaxLength(256);
        builder.Property(entity => entity.ActorEmail).HasMaxLength(256);
        builder.Property(entity => entity.RequestPath).HasMaxLength(512);
        builder.Property(entity => entity.DispatchStatus).HasDefaultValue((short)0);
        builder.Property(entity => entity.DispatchAttempts).HasDefaultValue(0);
        builder.Property(entity => entity.SchemaVersion).HasDefaultValue((short)1);

        builder.HasIndex(entity => new { entity.AggregateId, entity.AggregateRevision }).IsUnique();
        builder.HasIndex(entity => new { entity.AggregateId, entity.OccurredAtUtc, entity.Id });
        builder.HasIndex(entity => entity.CorrelationId);
        builder.HasIndex(entity => new { entity.DispatchStatus, entity.PersistedAtUtc, entity.Id });
        builder.HasIndex(entity => new { entity.ActorUserId, entity.OccurredAtUtc });
        builder.HasIndex(entity => new { entity.EventType, entity.OccurredAtUtc });
    }
}
