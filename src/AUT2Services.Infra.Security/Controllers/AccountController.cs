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

    [HttpPost("modificausuario")]
    [Authorize]
    public async Task<IActionResult> ModificaUsuario(ModificaUsuarioViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ModificaUsuarioAsync(viewModel, Request, Response));
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

}
