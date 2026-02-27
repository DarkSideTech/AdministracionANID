// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.337
// -------------------------------------------------
using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Services.API.Controllers;

[Route("api/[controller]")]
public class EnumerationController : ApiController
{

    [HttpGet("BuscarTodosLosValores_EnumEstadoDeUsuario")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumEstadoDeUsuario()
    {
        return EnumEstadoDeUsuario.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumTipoDeUsuario")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumTipoDeUsuario()
    {
        return EnumTipoDeUsuario.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumTipoDeEntidad")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumTipoDeEntidad()
    {
        return EnumTipoDeEntidad.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumNivelDeProceso")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumNivelDeProceso()
    {
        return EnumNivelDeProceso.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumComoDesplegarUrlDeProceso")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumComoDesplegarUrlDeProceso()
    {
        return EnumComoDesplegarUrlDeProceso.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumRolesBase")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumRolesBase()
    {
        return EnumRolesBase.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumUsuariosBase")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumUsuariosBase()
    {
        return EnumUsuariosBase.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumNacionalidad")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumNacionalidad()
    {
        return EnumNacionalidad.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumDocumentoDeIdentidad")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumDocumentoDeIdentidad()
    {
        return EnumDocumentoDeIdentidad.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumSexoDeclarativo")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumSexoDeclarativo()
    {
        return EnumSexoDeclarativo.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumSexoRegistral")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumSexoRegistral()
    {
        return EnumSexoRegistral.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumProcesosBase")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumProcesosBase()
    {
        return EnumProcesosBase.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumUnidadOrganizacionalBase")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumUnidadOrganizacionalBase()
    {
        return EnumUnidadOrganizacionalBase.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumOrganizacionBase")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumOrganizacionBase()
    {
        return EnumOrganizacionBase.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumAuthCookie")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumAuthCookie()
    {
        return EnumAuthCookie.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumBusinessClaimTypes")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumBusinessClaimTypes()
    {
        return EnumBusinessClaimTypes.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumPartialBusinessClaimTypes")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumPartialBusinessClaimTypes()
    {
        return EnumPartialBusinessClaimTypes.ObtenerListaValores();
    }

    [HttpGet("BuscarTodosLosValores_EnumAccessTokenType")]
    [Authorize]
    public IEnumerable<string> BuscarTodosLosValores_EnumAccessTokenType()
    {
        return EnumAccessTokenType.ObtenerListaValores();
    }

 
}

