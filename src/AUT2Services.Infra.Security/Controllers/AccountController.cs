using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Infra.Security.Accounts.ValidateEmail;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AUT2Services.Infra.Security.Controllers;

[ApiController]
[Route("api/account")]
public partial class AccountController : ApiController
{
    private readonly IMediatorHandler mediator;
    private readonly IAccountServiceApp accountServiceApp;

    public AccountController(
        IMediatorHandler mediator,
        IAccountServiceApp accountServiceApp)
    {
        this.mediator = mediator;
        this.accountServiceApp = accountServiceApp;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel dataViewModel)
    {
        var result = await accountServiceApp.RegisterAsync(dataViewModel);
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.RegisterAsync(dataViewModel));
    }

    [HttpPost("ValidateEmail")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateEmail(
        [FromBody] EmailConfirmationTokenViewModel request,
        CancellationToken cancellationToken
    )
    {
        var validateEmailCommand = new EmailConfirmationTokenCommand()
        {
            UserId = request.UserId,
            ConfirmationToken = request.ConfirmationToken
        };

        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await mediator.SendCommand(validateEmailCommand, cancellationToken));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.LoginAsync(viewModel));
    }

    [HttpPost("loginOrganizacion")]
    [Authorize]
    public async Task<IActionResult> LoginOrganizacion(LoginOrganizacionViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.LoginOrganizacionAsync(viewModel));
    }

    [HttpPost("refreshtoken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(RefreshTokenViewModel viewModel)
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.RefreshTokenAsync(viewModel));
    }

    [HttpGet("validarSesion")]
    [Authorize]
    public IActionResult ValidarSesion()
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(new { isAuthenticated = true, message = "Sesión válida" });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(await accountServiceApp.Logout());
    }

    [HttpGet("procesosautorizados")]
    [Authorize]
    public IEnumerable<string> ProcesosAutorizados()
    {
        return accountServiceApp.ProcesosAutorizados();
    }

    [HttpGet("rolesporproceso")]
    [Authorize]
    public IEnumerable<string> RolesPorProceso(string proceso)
    {
        return accountServiceApp.RolesPorProceso(proceso);
    }

    [HttpGet("datosusuario")]
    [Authorize]
    public DatosUsuarioDTO DatosUsuario()
    {
        return null;//accountServiceApp.RolesPorProceso();
    }
}


