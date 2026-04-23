using AUT2Services.Domain.Core.Time;

namespace AUT2Services.Domain.Security.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public DateTimeOffset? CreatedAtUtc { get; set; }
    public DateTimeOffset? ExpiresAtUtc { get; set; }
    public DateTimeOffset? RevokedAtUtc { get; set; }
    public string? SelectedOrganization { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public string? RevocationReason { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Usuario? User { get; set; }
    public Guid? Id_Entidad { get; set; }

    public bool IsExpired(IClock clock) => ExpiresAtUtc is not null && clock.UtcNow >= ExpiresAtUtc;
    public bool IsActive(IClock clock) => RevokedAtUtc is null && !IsExpired(clock);
}