using AUT2Services.Domain.Enumerations;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace AUT2Services.Infra.Security.Controllers;

[ApiController]
[Route("api/account")]
public partial class AccountController : ApiController
{
    private readonly IAccountServiceApp accountServiceApp;
    private readonly ICsrfService csrfService;

    public AccountController(
        IAccountServiceApp accountServiceApp,
        ICsrfService csrfService)
    {
        this.accountServiceApp = accountServiceApp;
        this.csrfService = csrfService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.RegisterAsync(viewModel, Request, Response));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.LoginAsync(viewModel, Request, Response));
    }

    [HttpPost("loginclaveunica")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginClaveUnica(LoginClaveUnicaViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.LoginClaveUnicaAsync(viewModel, Request, Response));
    }

    [HttpPost("loginorganizacion")]
    [Authorize]
    public async Task<IActionResult> LoginOrganizacion(LoginOrganizacionViewModel viewModel)
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.LoginOrganizacionAsync(viewModel, Request, Response, context));
    }

    [HttpPost("cambiounidadorganizacionalentidadrol")]
    [Authorize]
    public async Task<IActionResult> CambioUnidadOrganizacionalEntidadRol(CambioUnidadOrganizacionalEntidadRolViewModel viewModel)
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.CambioUnidadOrganizacionalEntidadRolAsync(viewModel, Request, Response, context));
    }

    [HttpPost("refreshtoken")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.RefreshTokenAsync(viewModel, Request, Response));
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.Logout(Request, Response));
    }

    [HttpPost("confirmemail")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateEmail([FromBody] ConfirmEmailRequest confirmEmailRequest)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ConfirmEmailAsync(confirmEmailRequest, Request, Response));
    }

    [HttpPost("resendconfirmationemail")]
    [AllowAnonymous]
    [EnableRateLimiting(EnumPolicyMaster.RESEND_CONFIRMATION_EMAIL)]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendEmailConfirmationTokenRequest resendEmailConfirmationTokenRequest)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ResendEmailConfirmationTokenAsync(resendEmailConfirmationTokenRequest, Request, Response));
    }

    [HttpGet("csrf")]
    [AllowAnonymous]
    public IActionResult Csrf()
    {
        Request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var existingToken);
        csrfService.EnsureTokenCookie(Response, existingToken);
        return NoContent();
    }

    [Authorize]
    [HttpGet("miinformacion")]
    public async Task<IActionResult> MiInformacion()
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.YoAsync(Request, Response, context));
    }

    [AllowAnonymous]
    [HttpGet("currentuser")]
    public async Task<IActionResult> CurrentUser()
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.CurrentUserAsync(Request, Response, context));
    }

    [HttpPost("buscarusuariospaginados")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> BuscarUsuariosPaginados(BuscarUsuariosPaginadosViewModel viewModel)
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.BuscarUsuariosPaginadosAsync(viewModel, Request, Response, context));
    }

    [HttpPost("modificausuario")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> ModificaUsuario(ModificaUsuarioViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ModificaUsuarioAsync(viewModel, Request, Response));
    }

    [HttpPut("activarusuario")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> ActivarUsuario(ActivarUsuarioViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ActivarUsuarioAsync(viewModel, Request, Response));
    }

    [HttpPut("desactivarusuario")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> DesactivarUsuario(DesactivarUsuarioViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.DesactivarUsuarioAsync(viewModel, Request, Response));
    }

    [HttpPost("adminmodificacorreoelectronico")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> AdminModificaCorreoElectronico(AdminModificaCorreoElectronicoViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.AdminModificaCorreoElectronicoAsync(viewModel, Request, Response));
    }

    [HttpPost("modificacorreoelectronico")]
    [Authorize]
    public async Task<IActionResult> ModificaCorreoElectronico(ModificaCorreoElectronicoViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ModificaCorreoElectronicoAsync(viewModel, Request, Response));
    }

    [HttpPost("solicitacambioclave")]
    [Authorize]
    public async Task<IActionResult> SolicitaCambioClave(SolicitaCambioClaveViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.SolicitaCambioClaveAsync(viewModel, Request, Response));
    }

    [HttpPost("reenviacodigocambioclave")]
    [Authorize]
    public async Task<IActionResult> ReenviaCodigoCambioClave(ReenviaCodigoCambioClaveViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ReenviaCodigoCambioClaveAsync(viewModel, Request, Response));
    }

    [HttpPost("confirmacambioclave")]
    [Authorize]
    public async Task<IActionResult> ConfirmaCambioClave(ConfirmaCambioClaveViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ConfirmaCambioClaveAsync(viewModel, Request, Response));
    }

    [HttpPost("solicitarecuperacionclave")]
    [AllowAnonymous]
    [EnableRateLimiting(EnumPolicyMaster.PASSWORD_RECOVERY)]
    public async Task<IActionResult> SolicitaRecuperacionClave(SolicitaRecuperacionClaveViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.SolicitaRecuperacionClaveAsync(viewModel, Request, Response));
    }

    [HttpPost("reenviacodigorecuperacionclave")]
    [AllowAnonymous]
    [EnableRateLimiting(EnumPolicyMaster.PASSWORD_RECOVERY)]
    public async Task<IActionResult> ReenviaCodigoRecuperacionClave(ReenviaCodigoRecuperacionClaveViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ReenviaCodigoRecuperacionClaveAsync(viewModel, Request, Response));
    }

    [HttpPost("confirmarecuperacionclave")]
    [AllowAnonymous]
    [EnableRateLimiting(EnumPolicyMaster.PASSWORD_RECOVERY)]
    public async Task<IActionResult> ConfirmaRecuperacionClave(ConfirmaRecuperacionClaveViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ConfirmaRecuperacionClaveAsync(viewModel, Request, Response));
    }

    [HttpPost("buscarrolespaginados")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> BuscarRolesPaginados(BuscarRolesPaginadosViewModel viewModel)
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.BuscarRolesPaginadosAsync(viewModel, Request, Response, context));
    }

    [HttpPost("buscarroles")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> BuscarRoles(BuscarRolesViewModel viewModel)
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.BuscarRolesAsync(viewModel, Request, Response, context));
    }

    [HttpPost("modificarol")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> ModificaRol(ModificaRolViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ModificaRolAsync(viewModel, Request, Response));
    }

    [HttpPut("activarrol")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> ActivarRol(ActivarRolViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ActivarRolAsync(viewModel, Request, Response));
    }

    [HttpPut("desactivarrol")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> DesactivarRol(DesactivarRolViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.DesactivarRolAsync(viewModel, Request, Response));
    }

    [HttpPut("requierevalidacionalserasignado")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> RequiereValidacionAlSerAsignado(RequiereValidacionAlSerAsignadoViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.RequiereValidacionAlSerAsignadoAsync(viewModel, Request, Response));
    }

    [HttpPut("norequierevalidacionalserasignado")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> NoRequiereValidacionAlSerAsignado(NoRequiereValidacionAlSerAsignadoViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.NoRequiereValidacionAlSerAsignadoAsync(viewModel, Request, Response));
    }

    [HttpPut("activavalidaciondeasignacionderoles")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> ActivaValidacionDeAsignacionDeRoles(ActivaValidacionDeAsignacionDeRolesViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ActivaValidacionDeAsignacionDeRolesAsync(viewModel, Request, Response));
    }

    [HttpPut("desactivavalidaciondeasignacionderoles")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> DesactivaValidacionDeAsignacionDeRoles(DesactivaValidacionDeAsignacionDeRolesViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.DesactivaValidacionDeAsignacionDeRolesAsync(viewModel, Request, Response));
    }

    [HttpPut("activadetalledeautorizaciones")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> ActivaDetalleDeAutorizaciones(ActivaDetalleDeAutorizacionesViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ActivaDetalleDeAutorizacionesAsync(viewModel, Request, Response));
    }

    [HttpPut("desactivadetalledeautorizaciones")]
    [Authorize(Policy = EnumPolicyMaster.ADMINISTRADOR)]
    public async Task<IActionResult> DesactivaDetalleDeAutorizaciones(DesactivaDetalleDeAutorizacionesViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.DesactivaDetalleDeAutorizacionesAsync(viewModel, Request, Response));
    }

}
