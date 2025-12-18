using AUT2Services.Domain.AI_Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AUT2Services.Infra.Security.Services;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsername()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(ClaimTypes.Name)!;
    }

    public string GetEmail()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(ClaimTypes.Email)!;
    }

    public string GetIdEntidad()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.ID_ENTIDAD)!;
    }

    public List<string> GetProcesos()
    {
        List<string> result = [];
        var procesos = _httpContextAccessor
        .HttpContext!
            .User
                .FindAll(EnumBusinessClaimTypes.PROCESO)!
                .ToList();

        foreach ( var item in procesos )
        {
            result.Add(item.Value);
        }

        return result;
    }

    public List<string> GetRolesPorProceso(string proceso)
    {
        List<string> result = [];
        var procesos = _httpContextAccessor
        .HttpContext!
            .User
                .FindAll(proceso)!
                .ToList();

        foreach (var item in procesos)
        {
            result.Add(item.Value);
        }

        return result;
    }

    public string GetIdUsuario()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.ID_USUARIO)!;
    }

    public string GetNombreADesplegar()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.NOMBRE_A_DESPLEGAR)!;
    }

    public string GetCodigoOrganizacion()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.CODIGO_ORGANIZACION)!;
    }

    public string GetNombreOrganizacion()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.NOMBRE_ORGANIZACION)!;
    }

    public string GetCodigoUnidadOrganizacional()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.CODIGO_UNIDAD_ORGANIZACIONAL)!;
    }

    public string GetNombreUnidadOrganizacional()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.NOMBRE_UNIDAD_ORGANIZACIONAL)!;
    }

    public string GetAccessTokenType()
    {
        return _httpContextAccessor
        .HttpContext!
            .User
                .FindFirstValue(EnumBusinessClaimTypes.ACCESS_TOKEN_TYPE)!;
    }
}