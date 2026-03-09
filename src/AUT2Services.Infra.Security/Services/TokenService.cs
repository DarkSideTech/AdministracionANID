using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AUT2Services.Infra.Security.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    private readonly JwtOptions jwtOptions;
    private readonly ILogger<TokenService> logger;
    private readonly ISecurityRepository securityRepository;
    private readonly IEntidadRepository entidadRepository;
    private readonly IOrganizacionRepository organizacionRepository;
    private readonly IUnidadOrganizacionalRepository unidadOrganizacionalRepository;
    private readonly IHttpContextAccessor httpContextAccessor;

    public TokenService(
        IConfiguration configuration,
        IOptions<JwtOptions> jwtOptions,
        ILogger<TokenService> logger,
        ISecurityRepository securityRepository,
        IEntidadRepository entidadRepository,
        IOrganizacionRepository organizacionRepository,
        IUnidadOrganizacionalRepository unidadOrganizacionalRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        this.configuration = configuration;
        this.jwtOptions = jwtOptions.Value;
        this.logger = logger;
        this.securityRepository = securityRepository;
        this.entidadRepository = entidadRepository;
        this.organizacionRepository = organizacionRepository;
        this.unidadOrganizacionalRepository = unidadOrganizacionalRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<(string jwtToken, DateTime expiresAtUtc)> GenerateJwtTokenLoginOrganizacion(Usuario user, Guid id_Entidad)
    {
        var entidad = await entidadRepository.BuscarPor_Id(id_Entidad) ?? throw new ArgumentException("La entidad indicada no existe, no es posible generar el token");
        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional) ?? throw new ArgumentException("La entidad indicada no esta asignado a una unidad organizacional valida");
        var organizacion = await organizacionRepository.BuscarPor_Id(unidadOrganizacional.Id_Organizacion) ?? throw new ArgumentException("La Organizacion asociada a la entidad no esparte de una organizacion valida");

        var claims = new List<Claim>
        {
            new(EnumBusinessClaimTypes.ID_USUARIO, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(EnumBusinessClaimTypes.NOMBRE_A_DESPLEGAR, user.NombreADesplegar!),
            new(EnumBusinessClaimTypes.CODIGO_ORGANIZACION, organizacion.Codigo!),
            new(EnumBusinessClaimTypes.NOMBRE_ORGANIZACION, organizacion.Nombre!),
            new(EnumBusinessClaimTypes.CODIGO_UNIDAD_ORGANIZACIONAL, unidadOrganizacional.Codigo!),
            new(EnumBusinessClaimTypes.NOMBRE_UNIDAD_ORGANIZACIONAL, unidadOrganizacional.Nombre!),
            new(EnumBusinessClaimTypes.ID_ENTIDAD, entidad.Id.ToString()),
            new(EnumBusinessClaimTypes.PROCESO, EnumProcesosBase.ADMINISTRACION)
        };

        IEnumerable<SecurityClaims> policies = null!;

        try
        {
            policies = await securityRepository.BuscarTodasLasPolicies(id_Entidad) ?? throw new ArgumentException("El usuario-entidad seleccionado no cuenta con ninguna politica de seguridad asignada");
            logger.LogInformation($"Policies details : {JsonConvert.SerializeObject(policies)}");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Error al buscar las politicas asociadas al uduario perfil");
            throw;
        }

        foreach (var policy in policies)
        {
            if (policy is not null)
            {
                claims.Add(new(policy.ClaimType, policy.ClaimValue));
            }
        }

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Secret));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(configuration["TiempoEnMinutosDeExpiracionDelToken"]!)),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationLoginOrganizationTokenTimeInMinutes);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwtToken, expires);
    }

    public (string jwtToken, DateTime expiresAtUtc) GenerateJwtTokenLogin(Usuario user)
    {
        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Secret));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(EnumBusinessClaimTypes.ID_USUARIO, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(EnumBusinessClaimTypes.NOMBRE_A_DESPLEGAR, user.NombreADesplegar!),
            new(EnumBusinessClaimTypes.PROCESO, EnumProcesosBase.ADMINISTRACION)
        };

        var expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationLoginTokenTimeInMinutes);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (jwtToken, expires);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration)
    {
        httpContextAccessor.HttpContext!.Response.Cookies.Append(cookieName,
            token, new CookieOptions
            {
                HttpOnly = true,
                Expires = expiration,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
    }

    public void DeleteAuthCookie(string cookieName)
    {
        httpContextAccessor.HttpContext!.Response.Cookies.Delete(cookieName);
    }

    public string GetAuthCookie(string cookieName)
    {
        if (httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue("NombreDeTuCookie", out var value) == true)
        {
            return value;
        }

        return string.Empty;
    }
}
