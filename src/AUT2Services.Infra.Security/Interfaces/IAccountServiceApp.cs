using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.DTOs;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Interfaces;

public interface IAccountServiceApp
{
    Task<CommandResponse> LoginAsync(LoginViewModel login, HttpRequest request, HttpResponse response);

    Task<CommandResponse> LoginOrganizacionAsync(LoginOrganizacionViewModel login, HttpRequest request, HttpResponse response, HttpContext httpContext);

    Task<CommandResponse> RefreshTokenAsync(RefreshTokenViewModel viewModel, HttpRequest request, HttpResponse response);

    Task<CommandResponse> RegisterAsync(RegisterViewModel viewModel, HttpRequest request, HttpResponse response);

    Task<CommandResponse> Logout(HttpRequest request, HttpResponse response);

    Task<CommandResponse> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest, HttpRequest request, HttpResponse response);

    Task<CommandResponse> ResendEmailConfirmationTokenAsync(ResendEmailConfirmationTokenRequest resendEmailConfirmationTokenRequest, HttpRequest request, HttpResponse response);

    Task<CommandResponse> YoAsync(HttpRequest request, HttpResponse response, HttpContext context);

    Task<CommandResponse> CurrentUserAsync(HttpRequest request, HttpResponse response, HttpContext context);

    IEnumerable<string> ProcesosAutorizados();
    IEnumerable<string> RolesPorProceso(string proceso);
    DatosUsuarioDTO DatosUsuario();
}
