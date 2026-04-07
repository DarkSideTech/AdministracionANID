namespace AUT2Services.Infra.Security.Models;

public class JwtOptions
{
    public const string JwtOptionsKey = "JwtOptions";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int LoginOrganizationTokenTimeInMinutes { get; set; } = 15;
    public int LoginTokenTimeInMinutes { get; set; } = 5;
    public int RefreshTokenDays { get; set; } = 7;
    public string[] AllowedCorsOrigins { get ; set ; } = [ "http://localhost:5002" ];
    public bool ImplementCookieOptionsSecure { get; set; } = false;
    public int MaximaCantidadIntentosFallidos { get; set; } = 5;
}