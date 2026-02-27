using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.DTOs;
using AUT2Services.Infra.Security.Accounts.ValidateEmail;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AUT2Services.Infra.Security.Controllers;

[ApiController]
[Route("api/account")]
public partial class AccountController : ApiController
{
    private readonly IMediatorHandler mediator;
    private readonly IAccountServiceApp accountServiceApp;
    private readonly SendEmailOptions sendEmailOptions;

    public AccountController(
        IMediatorHandler mediator,
        IAccountServiceApp accountServiceApp,
        IOptions<SendEmailOptions> sendEmailOptions)
    {
        this.mediator = mediator;
        this.accountServiceApp = accountServiceApp;
        this.sendEmailOptions = sendEmailOptions.Value;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel dataViewModel)
    {
        CommandResponse result = null!;
        try
        {
            result = await accountServiceApp.RegisterAsync(dataViewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return !ModelState.IsValid ? CustomResponse(ModelState) : CustomResponse(result);
    }

    [HttpGet("validateemail")]
    [AllowAnonymous]
    public async void ValidateEmail(string email, string validationtoken)
    {
        var validateEmailCommand = new EmailConfirmationTokenCommand()
        {
            Email = email,
            ConfirmationToken = validationtoken
        };

        var result = await mediator.SendCommand(validateEmailCommand);

        if (result.Result)
        {
            Redirect(sendEmailOptions.URLEmailValidate);
        }
        else 
        { 
            Redirect(sendEmailOptions.URLEmailNotValidate);
        }
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
        return accountServiceApp.DatosUsuario();
    }
}


