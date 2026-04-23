using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class NotificationOutboxMessageConfiguration : IEntityTypeConfiguration<NotificationOutboxMessage>
{
    public void Configure(EntityTypeBuilder<NotificationOutboxMessage> builder)
    {
        builder.ToTable("NotificationOutbox");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Channel).HasMaxLength(50).IsRequired();
        builder.Property(entity => entity.NotificationType).HasMaxLength(150).IsRequired();
        builder.Property(entity => entity.UserId).HasMaxLength(100);
        builder.Property(entity => entity.RequestPath).HasMaxLength(512);
        builder.Property(entity => entity.DeduplicationKey).HasMaxLength(300);
        builder.Property(entity => entity.PayloadJson).IsRequired();
        builder.Property(entity => entity.DispatchStatus).HasDefaultValue((short)0);
        builder.Property(entity => entity.DispatchAttempts).HasDefaultValue(0);
        builder.Property(entity => entity.SchemaVersion).HasDefaultValue((short)1);

        builder.HasIndex(entity => new { entity.DispatchStatus, entity.NextAttemptUtc, entity.CreatedAtUtc, entity.Id });
        builder.HasIndex(entity => new { entity.Channel, entity.NotificationType, entity.CreatedAtUtc });
        builder.HasIndex(entity => entity.UserId);
        builder.HasIndex(entity => entity.DeduplicationKey);
    }
}
