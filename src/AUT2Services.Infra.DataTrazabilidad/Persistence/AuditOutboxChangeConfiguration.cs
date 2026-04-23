using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class AuditOutboxChangeConfiguration : IEntityTypeConfiguration<AuditOutboxChange>
{
    public void Configure(EntityTypeBuilder<AuditOutboxChange> builder)
    {
        builder.ToTable("AuditOutboxChange");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Path).HasMaxLength(200).IsRequired();
        builder.Property(entity => entity.ValueType).HasMaxLength(512);

        builder.HasIndex(entity => entity.AuditOutboxMessageId);
        builder.HasIndex(entity => entity.Path);

        builder.HasOne(entity => entity.AuditOutboxMessage)
            .WithMany(message => message.Changes)
            .HasForeignKey(entity => entity.AuditOutboxMessageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
