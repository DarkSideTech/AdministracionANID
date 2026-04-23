using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class AuditAggregateCursorConfiguration : IEntityTypeConfiguration<AuditAggregateCursor>
{
    public void Configure(EntityTypeBuilder<AuditAggregateCursor> builder)
    {
        builder.ToTable("AuditAggregateCursor");
        builder.HasKey(entity => entity.AggregateId);

        builder.Property(entity => entity.LastRevision)
            .IsRequired();

        builder.Property(entity => entity.UpdatedAtUtc)
            .IsRequired();

        builder.Property(entity => entity.ConcurrencyToken)
            .IsRequired()
            .IsConcurrencyToken();
    }
}
