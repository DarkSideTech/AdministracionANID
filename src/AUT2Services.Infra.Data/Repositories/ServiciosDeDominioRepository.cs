using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Infra.Data.Repositories;

public class ServicioDeDominioRepository : IServicioDeDominioRepository
{
    private readonly AUT2ServicesContext db;
    private readonly ILogger<ServicioDeDominioRepository> logger;

    public ServicioDeDominioRepository(AUT2ServicesContext db, ILogger<ServicioDeDominioRepository> logger)
    {
        this.db = db;
        this.logger = logger;
    }


    public async Task<IEnumerable<UnidadOrganizacionalDTO>> BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario,
        Guid id_Organizacion
        )
    {
        IList<UnidadOrganizacionalDTO> result = [];

        var repositoryQuery =
            from entidad in db.Entidad

            join unidadOrganizacional in db.UnidadOrganizacional
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            where
                entidad.Id_Usuario == id_Usuario
                && unidadOrganizacional.Id_Organizacion == id_Organizacion

            select new UnidadOrganizacionalDTO
            {
                Id = unidadOrganizacional.Id,
                Id_Organizacion = unidadOrganizacional.Id_Organizacion,
                Codigo = unidadOrganizacional.Codigo,
                Nombre = unidadOrganizacional.Nombre,
                Descripcion = unidadOrganizacional.Descripcion,
                UnidadOrganizacionalBase = unidadOrganizacional.UnidadOrganizacionalBase,
                Activo = true
            };

        try
        {
            void action() => result = repositoryQuery
                    .AsNoTracking()
                    .ToList();
            await Task.Run(action);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al momento de obtener las unidades organizacionales por id_usuario [{id_Usuario}] y id_organizacion [{id_Organizacion}], error: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<EntidadParaAsignarPoliticaDTO>> BuscarEntidadesParaAsignarPolitica(
        Guid id_Usuario,
        Guid id_UnidadOrganizacional,
        DateTimeOffset fechaConsulta)
    {
        var usuario = await db.Users
            .AsNoTracking()
            .Where(user => user.Id == id_Usuario.ToString())
            .Select(user => new
            {
                user.NombreADesplegar,
                user.Email
            })
            .FirstOrDefaultAsync();

        if (usuario is null)
        {
            return [];
        }

        return await (
            from entidad in db.Entidad.AsNoTracking()

            join unidadOrganizacional in db.UnidadOrganizacional.AsNoTracking()
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            where entidad.Id_Usuario == id_Usuario
                && entidad.Id_UnidadOrganizacional == id_UnidadOrganizacional
                && (entidad.FechaInicioAutorizacion == null || entidad.FechaInicioAutorizacion <= fechaConsulta)
                && (entidad.FechaTerminoAutorizacion == null || entidad.FechaTerminoAutorizacion > fechaConsulta)

            orderby entidad.Principal descending,
                entidad.TipoDeEntidad,
                entidad.CorreoElectronico

            select new EntidadParaAsignarPoliticaDTO
            {
                IdEntidad = entidad.Id,
                IdUsuario = entidad.Id_Usuario,
                NombreUsuario = usuario.NombreADesplegar ?? usuario.Email ?? string.Empty,
                IdUnidadOrganizacional = unidadOrganizacional.Id,
                CodigoUnidadOrganizacional = unidadOrganizacional.Codigo,
                NombreUnidadOrganizacional = unidadOrganizacional.Nombre,
                TipoDeEntidad = entidad.TipoDeEntidad,
                CorreoElectronico = entidad.CorreoElectronico,
                Principal = entidad.Principal
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<EntidadParaAsignarPoliticaDTO>> BuscarEntidadesParaAsignarPoliticaPor_Id_Organizacion(
        Guid id_Organizacion,
        DateTimeOffset fechaConsulta,
        IReadOnlyCollection<Guid>? ids_UnidadesOrganizacionales)
    {
        var filtraUnidades = ids_UnidadesOrganizacionales is not null;
        if (filtraUnidades && ids_UnidadesOrganizacionales!.Count == 0)
        {
            return [];
        }

        var rows = await (
            from entidad in db.Entidad.AsNoTracking()

            join unidadOrganizacional in db.UnidadOrganizacional.AsNoTracking()
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            where unidadOrganizacional.Id_Organizacion == id_Organizacion
                && (!filtraUnidades || ids_UnidadesOrganizacionales!.Contains(unidadOrganizacional.Id))
                && (entidad.FechaInicioAutorizacion == null || entidad.FechaInicioAutorizacion <= fechaConsulta)
                && (entidad.FechaTerminoAutorizacion == null || entidad.FechaTerminoAutorizacion > fechaConsulta)

            select new
            {
                Entidad = entidad,
                UnidadOrganizacional = unidadOrganizacional
            })
            .ToListAsync();

        var idsUsuarios = rows
            .Select(item => item.Entidad.Id_Usuario.ToString())
            .Distinct()
            .ToArray();

        var usuarios = await db.Users
            .AsNoTracking()
            .Where(user => idsUsuarios.Contains(user.Id))
            .Select(user => new
            {
                user.Id,
                user.NombreADesplegar,
                user.Email
            })
            .ToDictionaryAsync(user => user.Id);

        return rows
            .Select(item =>
            {
                usuarios.TryGetValue(item.Entidad.Id_Usuario.ToString(), out var usuario);
                return new EntidadParaAsignarPoliticaDTO
                {
                    IdEntidad = item.Entidad.Id,
                    IdUsuario = item.Entidad.Id_Usuario,
                    NombreUsuario = usuario?.NombreADesplegar ?? usuario?.Email ?? string.Empty,
                    IdUnidadOrganizacional = item.UnidadOrganizacional.Id,
                    CodigoUnidadOrganizacional = item.UnidadOrganizacional.Codigo,
                    NombreUnidadOrganizacional = item.UnidadOrganizacional.Nombre,
                    TipoDeEntidad = item.Entidad.TipoDeEntidad,
                    CorreoElectronico = item.Entidad.CorreoElectronico,
                    Principal = item.Entidad.Principal
                };
            })
            .OrderBy(item => item.NombreUsuario)
            .ThenBy(item => item.CodigoUnidadOrganizacional)
            .ThenBy(item => item.TipoDeEntidad)
            .ToArray();
    }

    public async Task<IEnumerable<PoliticaAsignadaParaEntidadDTO>> BuscarPoliticasAsignadasPor_Id_Entidad(
        Guid id_Entidad,
        DateTimeOffset fechaConsulta)
    {
        var rows = await (
            from politicaAsignada in db.PoliticaAsignada.AsNoTracking()

            join proceso in db.Proceso.AsNoTracking()
                on politicaAsignada.Id_Proceso equals proceso.Id

            where politicaAsignada.Id_Entidad == id_Entidad
                && (politicaAsignada.FechaInicioAsignacion == null || politicaAsignada.FechaInicioAsignacion <= fechaConsulta)
                && (politicaAsignada.FechaTerminoAsignacion == null || politicaAsignada.FechaTerminoAsignacion > fechaConsulta)

            select new
            {
                PoliticaAsignada = politicaAsignada,
                Proceso = proceso
            })
            .ToListAsync();

        var idsRoles = rows
            .Select(item => item.PoliticaAsignada.Id_Rol.ToString())
            .Distinct()
            .ToArray();

        var roles = await db.Roles
            .AsNoTracking()
            .Where(rol => idsRoles.Contains(rol.Id))
            .Select(rol => new
            {
                rol.Id,
                rol.NormalizedName,
                rol.Name
            })
            .ToDictionaryAsync(rol => rol.Id);

        return rows
            .Select(item =>
            {
                roles.TryGetValue(item.PoliticaAsignada.Id_Rol.ToString(), out var rol);
                return new PoliticaAsignadaParaEntidadDTO
                {
                    IdPoliticaAsignada = item.PoliticaAsignada.Id,
                    IdEntidad = item.PoliticaAsignada.Id_Entidad,
                    IdRol = item.PoliticaAsignada.Id_Rol,
                    NombreRol = rol?.NormalizedName ?? rol?.Name ?? string.Empty,
                    IdProceso = item.PoliticaAsignada.Id_Proceso,
                    CodigoProceso = item.Proceso.Codigo,
                    NombreProceso = item.Proceso.Nombre,
                    RolRequiereValidacion = item.PoliticaAsignada.RolRequiereValidacion,
                    RolAsignadoValidado = item.PoliticaAsignada.RolAsignadoValidado
                };
            })
            .OrderBy(item => item.NombreRol)
            .ThenBy(item => item.CodigoProceso)
            .ToArray();
    }

    public async Task<IEnumerable<UnidadOrganizacionalAsignacionOrganizacionDTO>> BuscarUnidadesOrganizacionalesParaAsignarOrganizacion(
        Guid id_Organizacion)
    {
        var unidades = await (
            from unidadOrganizacional in db.UnidadOrganizacional.AsNoTracking()

            join organizacion in db.Organizacion.AsNoTracking()
                on unidadOrganizacional.Id_Organizacion equals organizacion.Id

            select new
            {
                UnidadOrganizacional = unidadOrganizacional,
                Organizacion = organizacion
            })
            .ToListAsync();

        var entidadesPrincipalesPorUnidad = await db.Entidad
            .AsNoTracking()
            .Where(entidad => entidad.Principal)
            .GroupBy(entidad => entidad.Id_UnidadOrganizacional)
            .Select(group => new
            {
                IdUnidadOrganizacional = group.Key,
                Cantidad = group.LongCount()
            })
            .ToDictionaryAsync(item => item.IdUnidadOrganizacional, item => item.Cantidad);

        return unidades
            .Select(item =>
            {
                entidadesPrincipalesPorUnidad.TryGetValue(item.UnidadOrganizacional.Id, out var cantidadEntidadesPrincipales);
                return new UnidadOrganizacionalAsignacionOrganizacionDTO
                {
                    IdUnidadOrganizacional = item.UnidadOrganizacional.Id,
                    IdOrganizacionActual = item.Organizacion.Id,
                    CodigoUnidadOrganizacional = item.UnidadOrganizacional.Codigo,
                    NombreUnidadOrganizacional = item.UnidadOrganizacional.Nombre,
                    DescripcionUnidadOrganizacional = item.UnidadOrganizacional.Descripcion,
                    UnidadOrganizacionalBase = item.UnidadOrganizacional.UnidadOrganizacionalBase,
                    Activo = item.UnidadOrganizacional.Activo,
                    CodigoOrganizacionActual = item.Organizacion.Codigo,
                    NombreOrganizacionActual = item.Organizacion.Nombre,
                    AsignadaAOrganizacion = item.Organizacion.Id == id_Organizacion,
                    TieneEntidadPrincipal = cantidadEntidadesPrincipales > 0,
                    CantidadEntidadesPrincipales = cantidadEntidadesPrincipales
                };
            })
            .OrderByDescending(item => item.AsignadaAOrganizacion)
            .ThenBy(item => item.CodigoOrganizacionActual)
            .ThenBy(item => item.CodigoUnidadOrganizacional)
            .ToArray();
    }

    public async Task<AsignacionesRolesPendientesValidacionPageDTO> BuscarAsignacionesRolesPendientesValidacion(
        Guid id_Usuario_Validador,
        Guid id_Rol_ValidaAsignacionUsuario,
        Guid id_Organizacion_ANID,
        int numeroDePagina,
        int cantidadPorPagina,
        string? busqueda,
        DateTimeOffset fechaConsulta)
    {
        var idsOrganizacionesValidador = await (
            from politicaAsignada in db.PoliticaAsignada.AsNoTracking()

            join entidad in db.Entidad.AsNoTracking()
                on politicaAsignada.Id_Entidad equals entidad.Id

            join unidadOrganizacional in db.UnidadOrganizacional.AsNoTracking()
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            where politicaAsignada.Id_Rol == id_Rol_ValidaAsignacionUsuario
                && politicaAsignada.RolAsignadoValidado
                && entidad.Id_Usuario == id_Usuario_Validador
                && (politicaAsignada.FechaInicioAsignacion == null || politicaAsignada.FechaInicioAsignacion <= fechaConsulta)
                && (politicaAsignada.FechaTerminoAsignacion == null || politicaAsignada.FechaTerminoAsignacion > fechaConsulta)

            select unidadOrganizacional.Id_Organizacion)
            .Distinct()
            .ToArrayAsync();

        if (idsOrganizacionesValidador.Length == 0)
        {
            return new AsignacionesRolesPendientesValidacionPageDTO(numeroDePagina, cantidadPorPagina, 0, []);
        }

        var validadorOperaDesdeANID = idsOrganizacionesValidador.Contains(id_Organizacion_ANID);

        var rows = await (
            from politicaAsignada in db.PoliticaAsignada.AsNoTracking()

            join entidad in db.Entidad.AsNoTracking()
                on politicaAsignada.Id_Entidad equals entidad.Id

            join unidadOrganizacional in db.UnidadOrganizacional.AsNoTracking()
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            join organizacion in db.Organizacion.AsNoTracking()
                on unidadOrganizacional.Id_Organizacion equals organizacion.Id

            join proceso in db.Proceso.AsNoTracking()
                on politicaAsignada.Id_Proceso equals proceso.Id

            join rol in db.Roles.AsNoTracking()
                on politicaAsignada.Id_Rol.ToString() equals rol.Id

            where politicaAsignada.RolRequiereValidacion
                && !politicaAsignada.RolAsignadoValidado
                && rol.RequiereValidacionDeAsignacion == true
                && (validadorOperaDesdeANID || idsOrganizacionesValidador.Contains(organizacion.Id))
                && (politicaAsignada.FechaInicioAsignacion == null || politicaAsignada.FechaInicioAsignacion <= fechaConsulta)
                && (politicaAsignada.FechaTerminoAsignacion == null || politicaAsignada.FechaTerminoAsignacion > fechaConsulta)

            select new
            {
                PoliticaAsignada = politicaAsignada,
                Entidad = entidad,
                UnidadOrganizacional = unidadOrganizacional,
                Organizacion = organizacion,
                Proceso = proceso,
                Rol = rol
            })
            .ToListAsync();

        var idsUsuarios = rows
            .Select(item => item.Entidad.Id_Usuario.ToString())
            .Distinct()
            .ToArray();

        var usuarios = await db.Users
            .AsNoTracking()
            .Where(user => idsUsuarios.Contains(user.Id))
            .Select(user => new
            {
                user.Id,
                user.NombreADesplegar,
                user.Email
            })
            .ToDictionaryAsync(user => user.Id);

        var items = rows
            .Select(item =>
            {
                usuarios.TryGetValue(item.Entidad.Id_Usuario.ToString(), out var usuario);
                return new AsignacionRolPendienteValidacionDTO
                {
                    IdPoliticaAsignada = item.PoliticaAsignada.Id,
                    IdEntidad = item.Entidad.Id,
                    IdUsuario = item.Entidad.Id_Usuario,
                    NombreUsuario = usuario?.NombreADesplegar ?? usuario?.Email ?? string.Empty,
                    CorreoElectronico = item.Entidad.CorreoElectronico,
                    IdOrganizacion = item.Organizacion.Id,
                    CodigoOrganizacion = item.Organizacion.Codigo,
                    NombreOrganizacion = item.Organizacion.Nombre,
                    IdUnidadOrganizacional = item.UnidadOrganizacional.Id,
                    CodigoUnidadOrganizacional = item.UnidadOrganizacional.Codigo,
                    NombreUnidadOrganizacional = item.UnidadOrganizacional.Nombre,
                    TipoDeEntidad = item.Entidad.TipoDeEntidad,
                    IdRol = item.PoliticaAsignada.Id_Rol,
                    NombreRol = item.Rol.NormalizedName ?? item.Rol.Name ?? string.Empty,
                    IdProceso = item.Proceso.Id,
                    CodigoProceso = item.Proceso.Codigo,
                    NombreProceso = item.Proceso.Nombre,
                    FechaCreacion = item.PoliticaAsignada.FechaCreacion,
                    FechaInicioAsignacion = item.PoliticaAsignada.FechaInicioAsignacion,
                    FechaTerminoAsignacion = item.PoliticaAsignada.FechaTerminoAsignacion,
                    RolRequiereValidacion = item.PoliticaAsignada.RolRequiereValidacion,
                    RolAsignadoValidado = item.PoliticaAsignada.RolAsignadoValidado
                };
            });

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var busquedaNormalizada = busqueda.Trim();
            items = items.Where(item =>
                item.NombreUsuario.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.CorreoElectronico.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.NombreRol.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.CodigoProceso.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.NombreProceso.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.CodigoOrganizacion.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.NombreOrganizacion.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.CodigoUnidadOrganizacional.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase) ||
                item.NombreUnidadOrganizacional.Contains(busquedaNormalizada, StringComparison.OrdinalIgnoreCase));
        }

        var orderedItems = items
            .OrderBy(item => item.NombreOrganizacion)
            .ThenBy(item => item.CodigoUnidadOrganizacional)
            .ThenBy(item => item.NombreUsuario)
            .ThenBy(item => item.NombreRol)
            .ThenBy(item => item.CodigoProceso)
            .ToArray();

        var total = orderedItems.LongLength;
        var pageItems = orderedItems
            .Skip((numeroDePagina - 1) * cantidadPorPagina)
            .Take(cantidadPorPagina)
            .ToArray();

        return new AsignacionesRolesPendientesValidacionPageDTO(numeroDePagina, cantidadPorPagina, total, pageItems);
    }

    public async Task<EntidadDTO?> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario,
        Guid id_Organizacion
        )
    {
        EntidadDTO? result = null;

        var repositoryQuery =
            from entidad in db.Entidad

            join unidadOrganizacional in db.UnidadOrganizacional
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            where
                entidad.Id_Usuario == id_Usuario
                && unidadOrganizacional.Id_Organizacion == id_Organizacion
                && entidad.Principal

            select new EntidadDTO
            {
                Id = entidad.Id,
                Id_UnidadOrganizacional = entidad.Id_UnidadOrganizacional,
                Id_Usuario = entidad.Id_Usuario,
                TipoDeEntidad = entidad.TipoDeEntidad,
                CorreoElectronico = entidad.CorreoElectronico,
                FechaInicioAutorizacion = entidad.FechaInicioAutorizacion,
                FechaTerminoAutorizacion = entidad.FechaTerminoAutorizacion,
                FechaCreacion = entidad.FechaCreacion,
                Principal = entidad.Principal,
                EntidadBase = entidad.EntidadBase
            };

        try
        {
            void action() => result = repositoryQuery
                                .AsNoTracking()
                                .FirstOrDefault();
            await Task.Run(action);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al momento de obtener las unidades organizacionales por id_usuario [{id_Usuario}] y id_organizacion [{id_Organizacion}], error: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UsuarioConRolValidaAsignacionDeRol(
        Guid id_Usuario, 
        Guid id_Rol_ValidaAsignacionUsuario, 
        Guid id_Organizacion, 
        Guid id_Organizacion_ANID)
    {
        bool result = false;

        var repositoryQuery =
            from politicaAsignada in db.PoliticaAsignada

            join entidad in db.Entidad
                on politicaAsignada.Id_Entidad equals entidad.Id

            join unidadOrganizacional in db.UnidadOrganizacional
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            join organizacion in db.Organizacion
                on unidadOrganizacional.Id_Organizacion equals organizacion.Id

            where
                politicaAsignada.Id_Rol == id_Rol_ValidaAsignacionUsuario
                && entidad.Id_Usuario == id_Usuario
                && (organizacion.Id == id_Organizacion
                    || organizacion.Id == id_Organizacion_ANID)

            select true;

        try
        {
            void action() => result = repositoryQuery
                                .FirstOrDefault();
            await Task.Run(action);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al momento de obtener las unidades organizacionales por id_usuario [{id_Usuario}] y id_organizacion [{id_Organizacion}], error: {ex.Message}");
        }

        return result;
    }

    public async Task<bool> UsuarioConRolValidaEnrrolamiento(
        Guid id_Usuario,
        Guid id_Rol_ValidaEnrrolamiento)
    {
        bool result = false;

        var repositoryQuery =
            from politicaAsignada in db.PoliticaAsignada

            join entidad in db.Entidad
                on politicaAsignada.Id_Entidad equals entidad.Id

            where
                politicaAsignada.Id_Rol == id_Rol_ValidaEnrrolamiento
                && entidad.Id_Usuario == id_Usuario

            select true;

        try
        {
            void action() => result = repositoryQuery
                                .FirstOrDefault();
            await Task.Run(action);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al momento de obtener las unidades organizacionales por id_usuario [{id_Usuario}] y id_Rol_ValidaEnrrolamiento [{id_Rol_ValidaEnrrolamiento}], error: {ex.Message}");
        }

        return result;
    }

    public async Task<IEnumerable<OrganizacionPorUsuarioDTO>> BuscarOrganizacionesPor_Id_Usuario(Guid id_Usuario)
    {
        IList<OrganizacionPorUsuarioDTO> result = [];

        var repositoryQuery =
            from entidad in db.Entidad

            join unidadOrganizacional in db.UnidadOrganizacional
                on entidad.Id_UnidadOrganizacional equals unidadOrganizacional.Id

            join org in db.Organizacion
                on unidadOrganizacional.Id_Organizacion equals org.Id

            where
                entidad.Id_Usuario == id_Usuario

            select new OrganizacionPorUsuarioDTO
            {
                Codigo_Organizacion = org.Codigo,
                Nombre_Organizacion = org.Nombre
            };

        try
        {
            void action() => result = repositoryQuery
                                .AsNoTracking()
                                .Distinct()
                                .ToList();
            await Task.Run(action);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al momento de obtener las organizaciones por id_usuario [{id_Usuario}], error: {ex.Message}");
        }

        return result;

    }
}
