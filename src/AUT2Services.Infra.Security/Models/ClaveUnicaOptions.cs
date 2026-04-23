namespace AUT2Services.Infra.Security.Models;

public class ClaveUnicaOptions
{
    public const string ClaveUnicaOptionsKey = "ClaveUnicaOptions";

    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RedirectUri { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = "https://accounts.claveunica.gob.cl/openid/token/";
    public string UserInfoUrl { get; set; } = "https://accounts.claveunica.gob.cl/openid/userinfo/";
}
