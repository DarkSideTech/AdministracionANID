using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Services;

public class SecurityRepository : ISecurityRepository
{
    private readonly AUT2ServicesContext db;
    private readonly ILogger<SecurityRepository> logger;

    public SecurityRepository(AUT2ServicesContext db, ILogger<SecurityRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public IUnitOfWork UnitOfWork => db;

    public async Task<Entidad> BuscarEntidadPrincipalPorUsuarioOrganizacion(Guid id_Usuario, Guid id_Organizacion)
    {
        Entidad? result = null;

        var perfilPrincipal =
            from organizacion in db.Organizacion

            join unidadOrganizacional in db.UnidadOrganizacional
                on organizacion.Id equals unidadOrganizacional.Id_Organizacion

            join entidad in db.Entidad
                on unidadOrganizacional.Id equals entidad.Id_UnidadOrganizacional

            where
                organizacion.Id == id_Organizacion
                && entidad.Principal
                && entidad.Id_Usuario == id_Usuario

            select new
            {
                Entidad = entidad
            };

        try
        {
            Action action = () =>
            {
                result = perfilPrincipal
                                .AsNoTracking()
                                .FirstOrDefault()!
                                .Entidad;
            };
            await Task.Run(action);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener el perfil principal asociado al usuario y la entidad: {ex.Message}");
            throw;
        }
        return result;

    }

    public async Task<IEnumerable<SecurityClaims>> BuscarTodasLasPolicies(Guid id_Entidad)
    {
        IList<SecurityClaims> result = [];

        var politicasAsignadas =
            from entidad in db.Entidad

            join politicaAsignada in db.PoliticaAsignada
                on entidad.Id equals politicaAsignada.Id_Entidad

            join rol in db.Roles
                on politicaAsignada.Id_Rol.ToString() equals rol.Id

            join proceso in db.Proceso
                on politicaAsignada.Id_Proceso equals proceso.Id

            where
                entidad.Id == id_Entidad
                && politicaAsignada.RolAsignadoValidado
                && (politicaAsignada.FechaInicioAsignacion <= DateTimeOffset.Now
                    && politicaAsignada.FechaTerminoAsignacion >= DateTimeOffset.Now)

            select new
            {
                NombreRol = rol.NormalizedName ?? "Missing Data",
                CodigoProceso = proceso.Codigo ?? "Missing Data",
                NombreProceso = proceso.Nombre ?? "Missing Data",
                Token = proceso.Token ?? "Missing Data",
                Url = proceso.Url ?? "Missing Data",
                ComoDesplegarUrl = proceso.ComoDesplegarUrlDeProceso ?? "Missing Data"
            };

        try
        {
            await Task.Run(() =>
            {
                var politicas = politicasAsignadas.AsNoTracking().ToList();
                var proceso = new List<String>() { };
                foreach (var politica in politicas)
                {
                    result.Add(new SecurityClaims
                    {
                        ClaimType = politica.CodigoProceso,
                        ClaimValue = politica.NombreRol
                    });

                    if (!proceso.Exists(p => p == politica.CodigoProceso))
                    {
                        result.Add(new SecurityClaims
                        {
                            ClaimType = EnumBusinessClaimTypes.PROCESO,
                            ClaimValue = politica.CodigoProceso
                        });

                        result.Add(new SecurityClaims
                        {
                            ClaimType = $"{EnumBusinessClaimTypes.PROCESO}{EnumPartialBusinessClaimTypes._NOMBRE}",
                            ClaimValue = politica.NombreProceso
                        });

                        result.Add(new SecurityClaims
                        {
                            ClaimType = $"{EnumBusinessClaimTypes.PROCESO}{EnumPartialBusinessClaimTypes._TOKEN}",
                            ClaimValue = politica.Token
                        });

                        result.Add(new SecurityClaims
                        {
                            ClaimType = $"{EnumBusinessClaimTypes.PROCESO}{EnumPartialBusinessClaimTypes._URL}",
                            ClaimValue = politica.Url
                        });

                        result.Add(new SecurityClaims
                        {
                            ClaimType = $"{EnumBusinessClaimTypes.PROCESO}{EnumPartialBusinessClaimTypes._COMO_DESPLEGAR_URL}",
                            ClaimValue = politica.ComoDesplegarUrl
                        });

                        proceso.Add(politica.CodigoProceso);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener las policies desde la base de datos: {ex.Message}");
            throw;
        }
        return result;
    }

    public async Task<int> BuscarUltimoIdAutorizacion()
    {
        var maxId = await db.RoleClaims
                            .MaxAsync(x => x.Id);

        return maxId;
    }

    public async Task<Usuario> BuscarUsuario(string userName)
    {
        var user = await db.Users
                            .FirstOrDefaultAsync(x => x.Email == userName);

        if (user is null)
        {
            user = await db.Users
                .FirstOrDefaultAsync(x => x.UserName == userName);

            if (user is null)
            {
                logger.LogWarning($"Usuario {userName} no existe");
            }
        }

        return user!;
    }

    public void Dispose()
    {
        db.Dispose();
    }

    public async Task<Usuario?> BuscarUsuarioPor_RefreshToken(string refreshToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.RefreshToken.Equals(refreshToken));

        return user;
    }
}