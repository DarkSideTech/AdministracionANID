using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Infra.Security.Accounts.CurrentUser;
using AUT2Services.Infra.Security.Accounts.Logout;
using AUT2Services.Infra.Security.Accounts.Yo;
using AUT2Services.Infra.Security.Extensions;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Services;

public class AccountServiceApp : IAccountServiceApp
{
    private readonly IMediatorHandler mediator;
    private readonly IUserAccessor userAccessor;

    public AccountServiceApp(
                IMediatorHandler mediator,
                IUserAccessor userAccessor
        )
    {
        this.mediator = mediator;
        this.userAccessor = userAccessor;
    }

    public Task<CommandResponse> LoginAsync(LoginViewModel login, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(login.ToLoginCommand(request, response));
    }

    public Task<CommandResponse> LoginOrganizacionAsync(LoginOrganizacionViewModel loginOrganizacion, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        return mediator.SendCommand(loginOrganizacion.ToLoginOrganizacionCommand(request, response, httpContext));
    }

    public Task<CommandResponse> RefreshTokenAsync(RefreshTokenViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToRefreshTokenCommand(request, response));
    }

    public Task<CommandResponse> RegisterAsync(RegisterViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        Task<CommandResponse> result = null!;
        try
        {
            result = mediator.SendCommand(viewModel.ToRegisterCommand(request, response));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return result;
    }

    public async Task<CommandResponse> Logout(HttpRequest request, HttpResponse response)
    {
        var logoutCommand = new LogoutCommand()
        {
            Request = request,
            Response = response
        };
        return await mediator.SendCommand(logoutCommand);
    }

    public Task<CommandResponse> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(confirmEmailRequest.ToEmailConfirmationTokenCommand(request, response));
    }

    public Task<CommandResponse> ResendEmailConfirmationTokenAsync(ResendEmailConfirmationTokenRequest resendEmailConfirmationTokenRequest, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(resendEmailConfirmationTokenRequest.ToResendEmailConfirmationTokenCommand(request, response));
    }

    public IEnumerable<string> ProcesosAutorizados()
    {
        return userAccessor.GetProcesos();
    }

    public IEnumerable<string> RolesPorProceso(string proceso)
    {
        return userAccessor.GetRolesPorProceso(proceso);
    }

    public Task<CommandResponse> YoAsync(HttpRequest request, HttpResponse response, HttpContext context)
    {
        return mediator.SendCommand(new YoCommand() { 
            Request = request, 
            Response = response, 
            Context = context 
        });
    }

    public Task<CommandResponse> CurrentUserAsync(HttpRequest request, HttpResponse response, HttpContext context)
    {
        return mediator.SendCommand(new CurrentUserCommand()
        {
            Request = request,
            Response = response,
            Context = context
        });
    }


    public DatosUsuarioDTO DatosUsuario()
    {
        DatosUsuarioDTO result = new()
        {
            NombreADesplegar = userAccessor.GetNombreADesplegar(),
            CodigoOrganizaicon = userAccessor.GetCodigoOrganizacion(),
            NombreOrganizaicon = userAccessor.GetNombreOrganizacion(),
            CodigoUnidadOrganizacional = userAccessor.GetCodigoUnidadOrganizacional(),
            NombreUnidadOrganizacional = userAccessor.GetNombreUnidadOrganizacional(),
        };

        var procesosActivos = new List<ProcesoActivoDTO>();

        foreach (var item in userAccessor.GetProcesos())
        {
            procesosActivos.Add(new ProcesoActivoDTO()
            {
                Codigo = item,
                Roles = userAccessor.GetRolesPorProceso(item),
                NombreProceso = userAccessor.GetProcesoNombre(item),
                Url = userAccessor.GetProcesoUrl(item),
                ComoDesplegarUrlDeProceso = userAccessor.GetProcesoComoDesplegarUrl(item)
            });
        }

        result.ProcesosActivos = procesosActivos;

        return result;
    }
}
