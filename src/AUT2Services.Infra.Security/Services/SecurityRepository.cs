using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Security.Services;

public class SecurityRepository : ISecurityRepository
{
    private readonly AUT2ServicesContext db;
    private readonly ILogger<SecurityRepository> logger;
    private readonly IClock clock;

    public SecurityRepository(AUT2ServicesContext db, ILogger<SecurityRepository> logger, IClock clock)
    {
        this.db = db;
        this.logger = logger;
        this.clock = clock;
    }

    public IUnitOfWork UnitOfWork => db;

    public async Task<Entidad?> BuscarEntidadPrincipalPorUsuarioOrganizacion(Guid id_Usuario, Guid id_Organizacion)
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
            result = await perfilPrincipal
                .AsNoTracking()
                .Select(item => item.Entidad)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener el perfil principal asociado al usuario y la entidad: {ex.Message}");
            throw;
        }
        return result;

    }

    public async Task<IEnumerable<SecurityClaims>> BuscarTodasLasPolicies(Guid id_Entidad, Guid id_Rol)
    {
        IList<SecurityClaims> result = [];
        var nowUtc = clock.UtcNow;

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
                && politicaAsignada.Id_Rol == id_Rol
                && politicaAsignada.RolAsignadoValidado
                && (politicaAsignada.FechaInicioAsignacion == null
                    || politicaAsignada.FechaInicioAsignacion <= nowUtc)
                && (politicaAsignada.FechaTerminoAsignacion == null
                    || politicaAsignada.FechaTerminoAsignacion >= nowUtc)

            select new ProcessAssignmentProjection
            {
                NombreRol = rol.NormalizedName ?? "Missing Data",
                IdProceso = proceso.Id,
                IdMacroProceso = proceso.IdMacro_Proceso == Guid.Empty ? (Guid?)null : proceso.IdMacro_Proceso,
                CodigoProceso = proceso.Codigo ?? "Missing Data",
                NombreProceso = proceso.Nombre ?? "Missing Data",
                NivelDeProceso = proceso.NivelDeProceso ?? "Missing Data",
                Token = proceso.Token ?? "Missing Data",
                Url = proceso.Url ?? "Missing Data",
                ComoDesplegarUrl = proceso.ComoDesplegarUrlDeProceso ?? "Missing Data"
            };

        try
        {
            var politicas = await politicasAsignadas.AsNoTracking().ToListAsync();
            var macroProcesoIds = politicas
                .Where(politica => politica.IdMacroProceso.HasValue)
                .Select(politica => politica.IdMacroProceso!.Value)
                .Distinct()
                .ToList();

            var macroProcesos = macroProcesoIds.Count == 0
                ? new Dictionary<Guid, MacroProcessProjection>()
                : await db.Proceso
                    .AsNoTracking()
                    .Where(proceso => macroProcesoIds.Contains(proceso.Id))
                    .Select(proceso => new MacroProcessProjection
                    {
                        Id = proceso.Id,
                        Codigo = proceso.Codigo ?? "Missing Data",
                        Nombre = proceso.Nombre ?? "Missing Data",
                        NivelDeProceso = proceso.NivelDeProceso ?? EnumNivelDeProceso.NIVEL_MACRO,
                        Token = proceso.Token ?? string.Empty,
                        Url = proceso.Url ?? string.Empty,
                        ComoDesplegarUrl = proceso.ComoDesplegarUrlDeProceso ?? string.Empty
                    })
                    .ToDictionaryAsync(proceso => proceso.Id);

            var procesosEmitidos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var politica in politicas)
            {
                var codigoProceso = politica.CodigoProceso.Trim();

                result.Add(new SecurityClaims
                {
                    ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._ROL}",
                    ClaimValue = politica.NombreRol
                });

                if (procesosEmitidos.Add(codigoProceso))
                {
                    AddProcessClaims(
                        result,
                        codigoProceso,
                        politica.IdProceso,
                        politica.IdMacroProceso,
                        politica.NombreProceso,
                        politica.NivelDeProceso,
                        politica.Token,
                        politica.Url,
                        politica.ComoDesplegarUrl);
                }

                if (string.Equals(politica.NivelDeProceso, EnumNivelDeProceso.NIVEL_SISTEMA, StringComparison.OrdinalIgnoreCase)
                    && politica.IdMacroProceso.HasValue
                    && macroProcesos.TryGetValue(politica.IdMacroProceso.Value, out var macroProceso)
                    && procesosEmitidos.Add(macroProceso.Codigo))
                {
                    AddProcessClaims(
                        result,
                        macroProceso.Codigo,
                        macroProceso.Id,
                        null,
                        macroProceso.Nombre,
                        macroProceso.NivelDeProceso,
                        macroProceso.Token,
                        macroProceso.Url,
                        macroProceso.ComoDesplegarUrl);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener las policies desde la base de datos: {ex.Message}");
            throw;
        }
        return result;
    }

    private static void AddProcessClaims(
        ICollection<SecurityClaims> result,
        string codigoProceso,
        Guid idProceso,
        Guid? idMacroProceso,
        string nombreProceso,
        string nivelDeProceso,
        string token,
        string url,
        string comoDesplegarUrl)
    {
        var macroId = ResolveMacroProcessId(idProceso, idMacroProceso, nivelDeProceso);

        result.Add(new SecurityClaims
        {
            ClaimType = EnumBusinessClaimTypes.PROCESO,
            ClaimValue = codigoProceso
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._ID_PROCESO}",
            ClaimValue = idProceso.ToString()
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._ID_MACRO_PROCESO}",
            ClaimValue = macroId
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._NOMBRE}",
            ClaimValue = nombreProceso
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._NIVEL_DE_PROCESO}",
            ClaimValue = nivelDeProceso
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._TOKEN}",
            ClaimValue = token
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._URL}",
            ClaimValue = url
        });

        result.Add(new SecurityClaims
        {
            ClaimType = $"{codigoProceso}{EnumPartialBusinessClaimTypes._COMO_DESPLEGAR_URL}",
            ClaimValue = comoDesplegarUrl
        });
    }

    private static string ResolveMacroProcessId(Guid idProceso, Guid? idMacroProceso, string nivelDeProceso)
    {
        if (string.Equals(nivelDeProceso, EnumNivelDeProceso.NIVEL_MACRO, StringComparison.OrdinalIgnoreCase))
        {
            return idProceso.ToString();
        }

        if (idMacroProceso.HasValue && idMacroProceso.Value != Guid.Empty)
        {
            return idMacroProceso.Value.ToString();
        }

        return string.Empty;
    }

    private sealed class ProcessAssignmentProjection
    {
        public string NombreRol { get; init; } = string.Empty;
        public Guid IdProceso { get; init; }
        public Guid? IdMacroProceso { get; init; }
        public string CodigoProceso { get; init; } = string.Empty;
        public string NombreProceso { get; init; } = string.Empty;
        public string NivelDeProceso { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public string ComoDesplegarUrl { get; init; } = string.Empty;
    }

    private sealed class MacroProcessProjection
    {
        public Guid Id { get; init; }
        public string Codigo { get; init; } = string.Empty;
        public string Nombre { get; init; } = string.Empty;
        public string NivelDeProceso { get; init; } = string.Empty;
        public string Token { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public string ComoDesplegarUrl { get; init; } = string.Empty;
    }

    public async Task<IList<string>> BuscarRolesPor_Id_Entidad(Guid id_Entidad)
    {
        IList<string> result = [];
        var nowUtc = clock.UtcNow;

        var rolesAsignados =
            from entidad in db.Entidad

            join politicaAsignada in db.PoliticaAsignada
                on entidad.Id equals politicaAsignada.Id_Entidad

            join rol in db.Roles
                on politicaAsignada.Id_Rol.ToString() equals rol.Id

            where
                entidad.Id == id_Entidad
                && politicaAsignada.RolAsignadoValidado
                && (politicaAsignada.FechaInicioAsignacion == null
                    || politicaAsignada.FechaInicioAsignacion <= nowUtc)
                && (politicaAsignada.FechaTerminoAsignacion == null
                    || politicaAsignada.FechaTerminoAsignacion >= nowUtc)

            select rol.NormalizedName ?? "Missing Data";

        try
        {
            result =
            [
                .. (await rolesAsignados.AsNoTracking().ToListAsync())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
            ];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener las unidades organizacionales para una entidad: {ex.Message}");
            throw;
        }
        return result;
    }

    public async Task<IList<UnidadOrganizacionalEntidadRolPorUsuario>> BuscarUnidadesOrganizacionalesEntidadRolPor_Id_Organizacion(Guid id_Organizacion, Guid id_Usuario)
    {
        List<UnidadOrganizacionalEntidadRolPorUsuario> result = [];
        var nowUtc = clock.UtcNow;

        var unidadesOrganizacionalesAsignadas =
            from organizacion in db.Organizacion

            join unidadOrganizacional in db.UnidadOrganizacional
                on organizacion.Id equals unidadOrganizacional.Id_Organizacion

            join entidad in db.Entidad
                on unidadOrganizacional.Id equals entidad.Id_UnidadOrganizacional

            join politicaAsignada in db.PoliticaAsignada
                on entidad.Id equals politicaAsignada.Id_Entidad

            join rol in db.Roles
                on politicaAsignada.Id_Rol.ToString() equals rol.Id

            where
                organizacion.Id == id_Organizacion
                && entidad.Id_Usuario == id_Usuario
                && politicaAsignada.RolAsignadoValidado
                && (politicaAsignada.FechaInicioAsignacion == null
                    || politicaAsignada.FechaInicioAsignacion <= nowUtc)
                && (politicaAsignada.FechaTerminoAsignacion == null
                    || politicaAsignada.FechaTerminoAsignacion >= nowUtc)

            select new UnidadOrganizacionalEntidadRolPorUsuario
            (
                Codigo_UnidadOrganizacional: unidadOrganizacional.Codigo,
                Nombre_UnidadOrganizacional: unidadOrganizacional.Nombre,
                Id_Entidad: entidad.Id.ToString(),
                Id_Rol: politicaAsignada.Id_Rol.ToString(),
                Nombre_Rol: rol.Name
            );

        try
        {
            result =
            [
                .. (await unidadesOrganizacionalesAsignadas.AsNoTracking().ToListAsync())
                        .Distinct()
                        .OrderBy(item => item.Codigo_UnidadOrganizacional, StringComparer.OrdinalIgnoreCase)
                        .ThenBy(item => item.Nombre_Rol, StringComparer.OrdinalIgnoreCase)
                        .ToList()
            ];
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener los roles para una entidad: {ex.Message}");
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

    public async Task<Entidad> BuscarRolesPorUsuarioEntidad(Guid id_Usuario, Guid id_Entidad)
    {
        Entidad? result = null;

        var perfilPrincipal =
            from organizacion in db.Organizacion

            join unidadOrganizacional in db.UnidadOrganizacional
                on organizacion.Id equals unidadOrganizacional.Id_Organizacion

            join entidad in db.Entidad
                on unidadOrganizacional.Id equals entidad.Id_UnidadOrganizacional

            where
                entidad.Id == id_Entidad
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


    public async Task<Usuario?> BuscarUsuarioPor_RefreshToken(string refreshToken)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

        return user;
    }

    public async Task<EntidadRolSeleccionado?> BuscarEntidadRolSeleccionadoPor_Id_Entidad(Guid id_Entidad, Guid? id_Rol = null)
    {
        var nowUtc = clock.UtcNow;
        var idRolSeleccionado = id_Rol ?? await BuscarIdRolPorDefectoPorEntidadAsync(id_Entidad, nowUtc);
        if (!idRolSeleccionado.HasValue)
        {
            return null;
        }

        var entidadRolSeleccionado =
            from entidad in db.Entidad

            join unidadOrganizacional in db.UnidadOrganizacional
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            join politicaAsignada in db.PoliticaAsignada
                on entidad.Id equals politicaAsignada.Id_Entidad

            where
                entidad.Id == id_Entidad
                && politicaAsignada.Id_Rol == idRolSeleccionado.Value
                && politicaAsignada.RolAsignadoValidado
                && (politicaAsignada.FechaInicioAsignacion == null
                    || politicaAsignada.FechaInicioAsignacion <= nowUtc)
                && (politicaAsignada.FechaTerminoAsignacion == null
                    || politicaAsignada.FechaTerminoAsignacion >= nowUtc)

            select new EntidadRolSeleccionado
            {
                Codigo_UnidadOrganizacional = unidadOrganizacional.Codigo,
                Id_Entidad = entidad.Id,
                Id_Rol = politicaAsignada.Id_Rol,
            };

        try
        {
            return await entidadRolSeleccionado
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al momento de obtener el perfil principal asociado al usuario y la entidad: {ex.Message}");
            throw;
        }
    }

    private async Task<Guid?> BuscarIdRolPorDefectoPorEntidadAsync(Guid id_Entidad, DateTimeOffset nowUtc)
    {
        var primeraAsignacionDeProceso =
            from entidad in db.Entidad

            join politicaAsignada in db.PoliticaAsignada
                on entidad.Id equals politicaAsignada.Id_Entidad

            join proceso in db.Proceso
                on politicaAsignada.Id_Proceso equals proceso.Id

            join rol in db.Roles
                on politicaAsignada.Id_Rol.ToString() equals rol.Id

            where
                entidad.Id == id_Entidad
                && politicaAsignada.RolAsignadoValidado
                && (politicaAsignada.FechaInicioAsignacion == null
                    || politicaAsignada.FechaInicioAsignacion <= nowUtc)
                && (politicaAsignada.FechaTerminoAsignacion == null
                    || politicaAsignada.FechaTerminoAsignacion >= nowUtc)

            orderby
                proceso.Codigo,
                rol.NormalizedName

            select politicaAsignada.Id_Rol;

        return await primeraAsignacionDeProceso.FirstOrDefaultAsync();
    }

}
