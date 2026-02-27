namespace AUT2Services.Infra.Security.Models;

public class JwtOptions
{
    public const string JwtOptionsKey = "JwtOptions";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationLoginOrganizationTokenTimeInMinutes { get; set; } = 10080;
    public int ExpirationLoginTokenTimeInMinutes { get; set; } = 7;
    public int ExpirationRefreshTokenTimeInMinutes { get; set; } = 7;
    public string[] AllowedCorsOrigins { get ; set ; } = [ "http://localhost:5002" ];
}