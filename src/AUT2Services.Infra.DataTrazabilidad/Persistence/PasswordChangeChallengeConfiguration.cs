using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.DataTrazabilidad.Persistence;

public sealed class PasswordChangeChallengeConfiguration : IEntityTypeConfiguration<PasswordChangeChallenge>
{
    public void Configure(EntityTypeBuilder<PasswordChangeChallenge> builder)
    {
        builder.ToTable("PasswordChangeChallenges");
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.UserId).HasMaxLength(100).IsRequired();
        builder.Property(entity => entity.CodeHash).HasMaxLength(256).IsRequired();
        builder.Property(entity => entity.RequestPath).HasMaxLength(512);
        builder.Property(entity => entity.FailedAttempts).HasDefaultValue(0);
        builder.Property(entity => entity.ResendCount).HasDefaultValue(0);

        builder.HasIndex(entity => new { entity.UserId, entity.CreatedAtUtc });
        builder.HasIndex(entity => new { entity.UserId, entity.ExpiresAtUtc, entity.ConsumedAtUtc, entity.CancelledAtUtc });
    }
}
