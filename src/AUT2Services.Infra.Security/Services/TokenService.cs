using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
    private readonly UserManager<Usuario> userManager;
    private readonly IServicioDeDominioRepository servicioDeDominioRepository;
    private readonly AUT2ServicesContext aUT2ServicesContext;
    private readonly IClock clock;

    public TokenService(
        IConfiguration configuration,
        IOptions<JwtOptions> jwtOptions,
        ILogger<TokenService> logger,
        ISecurityRepository securityRepository,
        IEntidadRepository entidadRepository,
        IOrganizacionRepository organizacionRepository,
        IUnidadOrganizacionalRepository unidadOrganizacionalRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<Usuario> userManager,
        IServicioDeDominioRepository servicioDeDominioRepository,
        AUT2ServicesContext aUT2ServicesContext,
        IClock clock)
    {
        this.configuration = configuration;
        this.jwtOptions = jwtOptions.Value;
        this.logger = logger;
        this.securityRepository = securityRepository;
        this.entidadRepository = entidadRepository;
        this.organizacionRepository = organizacionRepository;
        this.unidadOrganizacionalRepository = unidadOrganizacionalRepository;
        this.httpContextAccessor = httpContextAccessor;
        this.userManager = userManager;
        this.servicioDeDominioRepository = servicioDeDominioRepository;
        this.aUT2ServicesContext = aUT2ServicesContext;
        this.clock = clock;
    }

    public async Task<AccessTokenResult> GenerateAccessTokenAsync(Usuario user, string sessionId, Guid? idEntidad = null, Guid? idRol = null)
    {
        var expiresAtUtc = clock.UtcNow.UtcDateTime.AddMinutes(jwtOptions.LoginTokenTimeInMinutes);
        var securityStamp = await userManager.GetSecurityStampAsync(user);

        Entidad? entidad = null;
        UnidadOrganizacional? unidadOrganizacional = null;
        Organizacion? organizacion = null;
        EntidadRolSeleccionado? selectedEntityRole = null;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Sid, sessionId),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(EnumBusinessClaimTypes.ID_USUARIO, user.Id),
            new(EnumBusinessClaimTypes.NOMBRE_A_DESPLEGAR, user.NombreADesplegar!)
        };

        if (idEntidad == null)
        {
            entidad = await entidadRepository.BuscarPor_Id_Usuario_TipoDeEntidad_Persona(Guid.Parse(user.Id)) ?? throw new ArgumentException("La entidad dde tipo persona base del usuario no existe, no es posible generar el token");

            claims.Add(new(EnumBusinessClaimTypes.ID_ENTIDAD, entidad.Id.ToString()));
            claims.Add(new(EnumBusinessClaimTypes.PROCESO, EnumProcesosBase.ADMINISTRACION));
            claims.Add(new(EnumProcesosBase.ADMINISTRACION, EnumRolesBase.ADMINISTRADOR_ENTIDAD));
            claims.Add(new(ClaimTypes.Role, EnumRolesBase.ADMINISTRADOR_ENTIDAD));
        }
        else
        {
            entidad = await entidadRepository.BuscarPor_Id((Guid)idEntidad) ?? throw new ArgumentException("La entidad indicada no existe, no es posible generar el token");
            unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(entidad.Id_UnidadOrganizacional) ?? throw new ArgumentException("La entidad indicada no esta asignado a una unidad organizacional valida");
            organizacion = await organizacionRepository.BuscarPor_Id(unidadOrganizacional.Id_Organizacion) ?? throw new ArgumentException("La Organizacion asociada a la entidad no esparte de una organizacion valida");
            selectedEntityRole = await securityRepository.BuscarEntidadRolSeleccionadoPor_Id_Entidad(entidad.Id, idRol)
                ?? throw new ArgumentException("La entidad indicada no cuenta con un rol seleccionado valido");
            var selectedRoleName = await GetSelectedRoleNameAsync(selectedEntityRole.Id_Rol);

            claims.Add(new(EnumBusinessClaimTypes.ID_ENTIDAD, entidad.Id.ToString()));
            claims.Add(new(EnumBusinessClaimTypes.ID_ROL, selectedEntityRole.Id_Rol.ToString()));
            claims.Add(new(EnumBusinessClaimTypes.CODIGO_ORGANIZACION, organizacion.Codigo));
            claims.Add(new(EnumBusinessClaimTypes.NOMBRE_ORGANIZACION, organizacion.Nombre!));
            claims.Add(new(EnumBusinessClaimTypes.CODIGO_UNIDAD_ORGANIZACIONAL, unidadOrganizacional.Codigo));
            claims.Add(new(EnumBusinessClaimTypes.NOMBRE_UNIDAD_ORGANIZACIONAL, unidadOrganizacional.Nombre));

            if (string.IsNullOrWhiteSpace(selectedRoleName))
            {
                throw new ArgumentException("No fue posible determinar el rol activo del contexto seleccionado.");
            }

            claims.Add(new(ClaimTypes.Role, selectedRoleName));
        }

        if (!string.IsNullOrWhiteSpace(securityStamp))
        {
            claims.Add(new Claim(EnumTokenValidationClaims.SecurityStamp, securityStamp));
        }

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Key)
        );

        var tokenDescriptor = new JwtSecurityToken(
           issuer: jwtOptions.Issuer,
           audience: jwtOptions.Audience,
           claims: claims,
           expires: expiresAtUtc,
           signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        return new AccessTokenResult(accessToken, expiresAtUtc); 
    }

    public RefreshTokenIssuanceResult CreateRefreshToken(string sessionId, string? selectedOrganization = null)
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        var refreshTokenDays = jwtOptions.RefreshTokenDays;
        var rawToken = Base64UrlEncoder.Encode(bytes);
        var nowUtc = clock.UtcNow.UtcDateTime;

        return new RefreshTokenIssuanceResult(
            rawToken,
            new RefreshToken
            {
                SessionId = sessionId,
                TokenHash = HashRefreshToken(rawToken),
                SelectedOrganization = selectedOrganization,
                CreatedAtUtc = nowUtc,
                ExpiresAtUtc = nowUtc.AddDays(refreshTokenDays)
            }
        );
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToHexString(bytes);
    }

    public async Task<IList<OrganizacionPorUsuario>> BuscarOrganizacionesPorIdUsuario(string idUsuario)
    {
        List<OrganizacionPorUsuario> organizacionesPorusuario = [];

        var resultBuscarOrganizacionesPor_Id_Usuario = await servicioDeDominioRepository.BuscarOrganizacionesPor_Id_Usuario(Guid.Parse(idUsuario));

        if (resultBuscarOrganizacionesPor_Id_Usuario.Any())
        {
            foreach (var item in resultBuscarOrganizacionesPor_Id_Usuario)
            {
                organizacionesPorusuario.Add(new OrganizacionPorUsuario(
                
                    item.Codigo_Organizacion,
                    item.Nombre_Organizacion
                ));
            }
        }

        return organizacionesPorusuario;
    }

    public async Task<UserDto> CreateUserDtoAsync(
        Usuario user,
        Guid? id_Entidad = null,
        EntidadRolSeleccionado? entidadRolSeleccionado = null)
    {
        var informacionAdicional = user.InformacionAdicional.ToInformacionAdicionalModel();
        var entidad = id_Entidad.HasValue
            ? await entidadRepository.BuscarPor_Id(id_Entidad.Value)
            : null;
        var unidadesOrganizacionales = entidad is null
            ? []
            : (await GetUnidadesOrganizacionalesUsuarioAsync(user.Id, entidad.Id_UnidadOrganizacional))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        var selectedRole = await GetSelectedRoleNameAsync(entidadRolSeleccionado?.Id_Rol);
        IList<string> roles = string.IsNullOrWhiteSpace(selectedRole)
            ? []
            : [selectedRole];

        return new UserDto(
            Id: user.Id,
            Email: user.Email ?? string.Empty,
            NombreADesplegar: user.NombreADesplegar ?? string.Empty,
            NumeroDeTelefono: user.PhoneNumber ?? string.Empty,
            TipoDeUsuario: user.TipoDeUsuario ?? string.Empty,
            Nacionalidad: informacionAdicional.Nacionalidad ?? string.Empty,
            DocumentoDeIdentidad: informacionAdicional.DocumentoDeIdentidad ?? string.Empty,
            NumeroDeDocumento: informacionAdicional.NumeroDeDocumento ?? string.Empty,
            CodigoValidadorDocumento: informacionAdicional.CodigoValidadorDocumento ?? string.Empty,
            PrimerNombre: informacionAdicional.PrimerNombre ?? string.Empty,
            SegundoNombre: informacionAdicional.SegundoNombre ?? string.Empty,
            PrimerApellido: informacionAdicional.PrimerApellido ?? string.Empty,
            SegundoApellido: informacionAdicional.SegundoApellido ?? string.Empty,
            SexoDeclarativo: informacionAdicional.SexoDeclarativo ?? string.Empty,
            SexoRegistral: informacionAdicional.SexoRegistral ?? string.Empty,
            FechaDeNacimiento: informacionAdicional.FechaDeNacimiento,
            Roles: roles,
            UnidadesOrganizacionales: unidadesOrganizacionales);
    }

    private async Task<string?> GetSelectedRoleNameAsync(Guid? idRol)
    {
        if (!idRol.HasValue)
        {
            return null;
        }

        var roleId = idRol.Value.ToString();

        return await aUT2ServicesContext.Roles
            .AsNoTracking()
            .Where(role => role.Id == roleId)
            .Select(role => role.NormalizedName ?? role.Name)
            .FirstOrDefaultAsync();
    }

    private async Task<IList<string>> GetUnidadesOrganizacionalesUsuarioAsync(string userId, Guid idUnidadOrganizacional)
    {
        var unidadOrganizacional = await unidadOrganizacionalRepository.BuscarPor_Id(idUnidadOrganizacional);
        if (unidadOrganizacional is null)
        {
            return [];
        }

        var unidades = await securityRepository.BuscarUnidadesOrganizacionalesEntidadRolPor_Id_Organizacion(
            unidadOrganizacional.Id_Organizacion,
            Guid.Parse(userId));

        return unidades
            .Select(item => item.Codigo_UnidadOrganizacional)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Cast<string>()
            .ToList();
    }

    public async Task RevokeSessionAsync(string sessionId, string reason)
    {
        var nowUtc = clock.UtcNow.UtcDateTime;
        var sessionTokens = await aUT2ServicesContext.RefreshTokens
            .AsTracking()
            .Where(x => x.SessionId == sessionId && x.ExpiresAtUtc > nowUtc)
            .ToListAsync();

        if (sessionTokens.Count == 0)
        {
            return;
        }

        foreach (var token in sessionTokens.Where(x => x.RevokedAtUtc is null))
        {
            token.RevokedAtUtc = nowUtc;
            token.RevocationReason = reason;
        }

        aUT2ServicesContext.RefreshTokens.UpdateRange(sessionTokens);

        await aUT2ServicesContext.SaveChangesAsync();
    }
}

