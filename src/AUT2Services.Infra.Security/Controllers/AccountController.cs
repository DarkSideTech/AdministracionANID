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

    [HttpGet("csrf")]
    [AllowAnonymous]
    public IActionResult Csrf()
    {
        Request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var existingToken);
        csrfService.EnsureTokenCookie(Response, existingToken);
        return NoContent();
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

    [HttpPost("loginOrganizacion")]
    [Authorize]
    public async Task<IActionResult> LoginOrganizacion(LoginOrganizacionViewModel viewModel)
    {
        var context = HttpContext;
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.LoginOrganizacionAsync(viewModel, Request, Response, context));
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

    [HttpPost("ConfirmEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateEmail([FromBody] ConfirmEmailRequest confirmEmailRequest)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ConfirmEmailAsync(confirmEmailRequest, Request, Response));
    }

    [HttpPost("ResendConfirmationEmail")]
    [AllowAnonymous]
    [EnableRateLimiting("ResendConfirmationEmail")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendEmailConfirmationTokenRequest resendEmailConfirmationTokenRequest)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.ResendEmailConfirmationTokenAsync(resendEmailConfirmationTokenRequest, Request, Response));
    }

    [Authorize]
    [HttpGet("yo")]
    public async Task<IActionResult> yo()
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
}