namespace AUT2Services.Domain.Security.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? SelectedOrganization { get; set; }
    public string? ReplacedByTokenHash { get; set; }
    public string? RevocationReason { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Usuario? User { get; set; }
    public Guid? Id_Entidad { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAtUtc;
    public bool IsActive => RevokedAtUtc is null && !IsExpired;}

