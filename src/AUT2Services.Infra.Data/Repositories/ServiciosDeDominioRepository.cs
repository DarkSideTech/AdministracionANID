using AUT2Services.Domain.DTOs;
using AUT2Services.Domain.Entities;
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
        IList<UnidadOrganizacionalDTO> result = null!;

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

    public async Task<EntidadDTO> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(
        Guid id_Usuario,
        Guid id_Organizacion
        )
    {
        EntidadDTO result = null!;

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

    public async Task<bool> UsuarioConRolValidaEnnrrolamiento(
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
        IList<OrganizacionPorUsuarioDTO> result = null!;

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
