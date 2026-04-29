using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;
using AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;
using AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.CurrentUser;
using AUT2Services.Infra.Security.Accounts.Logout;
using AUT2Services.Infra.Security.Accounts.BuscarUsuariosPaginados;
using AUT2Services.Infra.Security.Accounts.ActivarUsuario;
using AUT2Services.Infra.Security.Accounts.DesactivarUsuario;
using AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.Roles;
using AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;
using AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;
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

    public AccountServiceApp(
                IMediatorHandler mediator
        )
    {
        this.mediator = mediator;
    }

    public Task<CommandResponse> LoginAsync(LoginViewModel login, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(login.ToLoginCommand(request, response));
    }

    public Task<CommandResponse> LoginClaveUnicaAsync(LoginClaveUnicaViewModel login, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(login.ToLoginClaveUnicaCommand(request, response));
    }

    public Task<CommandResponse> LoginOrganizacionAsync(LoginOrganizacionViewModel loginOrganizacion, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        return mediator.SendCommand(loginOrganizacion.ToLoginOrganizacionCommand(request, response, httpContext));
    }

    public Task<CommandResponse> CambioUnidadOrganizacionalEntidadRolAsync(CambioUnidadOrganizacionalEntidadRolViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        return mediator.SendCommand(viewModel.ToCambioUnidadOrganizacionalEntidadRolCommand(request, response, httpContext));
    }

    public Task<CommandResponse> RefreshTokenAsync(RefreshTokenViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToRefreshTokenCommand(request, response));
    }

    public Task<CommandResponse> RegisterAsync(RegisterViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToRegisterCommand(request, response)); 
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

    public Task<CommandResponse> BuscarUsuariosPaginadosAsync(BuscarUsuariosPaginadosViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext context)
    {
        return mediator.SendCommand(viewModel.ToBuscarUsuariosPaginadosCommand(request, response, context));
    }

    public Task<CommandResponse> ModificaUsuarioAsync(ModificaUsuarioViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToModificaUsuarioCommand(request, response));
    }

    public Task<CommandResponse> ActivarUsuarioAsync(ActivarUsuarioViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToActivarUsuarioCommand(request, response));
    }

    public Task<CommandResponse> DesactivarUsuarioAsync(DesactivarUsuarioViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToDesactivarUsuarioCommand(request, response));
    }

    public Task<CommandResponse> AdminModificaCorreoElectronicoAsync(AdminModificaCorreoElectronicoViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToAdminModificaCorreoElectronicoCommand(request, response));
    }

    public Task<CommandResponse> ModificaCorreoElectronicoAsync(ModificaCorreoElectronicoViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToModificaCorreoElectronicoCommand(request, response));
    }

    public Task<CommandResponse> SolicitaCambioClaveAsync(SolicitaCambioClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToSolicitaCambioClaveCommand(request, response));
    }

    public Task<CommandResponse> ReenviaCodigoCambioClaveAsync(ReenviaCodigoCambioClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToReenviaCodigoCambioClaveCommand(request, response));
    }

    public Task<CommandResponse> ConfirmaCambioClaveAsync(ConfirmaCambioClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToConfirmaCambioClaveCommand(request, response));
    }

    public Task<CommandResponse> SolicitaRecuperacionClaveAsync(SolicitaRecuperacionClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToSolicitaRecuperacionClaveCommand(request, response));
    }

    public Task<CommandResponse> ReenviaCodigoRecuperacionClaveAsync(ReenviaCodigoRecuperacionClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToReenviaCodigoRecuperacionClaveCommand(request, response));
    }

    public Task<CommandResponse> ConfirmaRecuperacionClaveAsync(ConfirmaRecuperacionClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToConfirmaRecuperacionClaveCommand(request, response));
    }

    public Task<CommandResponse> BuscarRolesPaginadosAsync(BuscarRolesPaginadosViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext context)
    {
        return mediator.SendCommand(viewModel.ToBuscarRolesPaginadosCommand(request, response, context));
    }

    public Task<CommandResponse> ModificaRolAsync(ModificaRolViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToModificaRolCommand(request, response));
    }

    public Task<CommandResponse> ActivarRolAsync(ActivarRolViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToActivarRolCommand(request, response));
    }

    public Task<CommandResponse> DesactivarRolAsync(DesactivarRolViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToDesactivarRolCommand(request, response));
    }

    public Task<CommandResponse> RequiereValidacionAlSerAsignadoAsync(RequiereValidacionAlSerAsignadoViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToRequiereValidacionAlSerAsignadoCommand(request, response));
    }

    public Task<CommandResponse> NoRequiereValidacionAlSerAsignadoAsync(NoRequiereValidacionAlSerAsignadoViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToNoRequiereValidacionAlSerAsignadoCommand(request, response));
    }

    public Task<CommandResponse> ActivaValidacionDeAsignacionDeRolesAsync(ActivaValidacionDeAsignacionDeRolesViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToActivaValidacionDeAsignacionDeRolesCommand(request, response));
    }

    public Task<CommandResponse> DesactivaValidacionDeAsignacionDeRolesAsync(DesactivaValidacionDeAsignacionDeRolesViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToDesactivaValidacionDeAsignacionDeRolesCommand(request, response));
    }

    public Task<CommandResponse> ActivaDetalleDeAutorizacionesAsync(ActivaDetalleDeAutorizacionesViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToActivaDetalleDeAutorizacionesCommand(request, response));
    }

    public Task<CommandResponse> DesactivaDetalleDeAutorizacionesAsync(DesactivaDetalleDeAutorizacionesViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        return mediator.SendCommand(viewModel.ToDesactivaDetalleDeAutorizacionesCommand(request, response));
    }
}
