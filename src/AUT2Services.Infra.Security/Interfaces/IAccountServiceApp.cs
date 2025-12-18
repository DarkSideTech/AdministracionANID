using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.DTOs;
using AUT2Services.Infra.Security.ViewModels;

namespace AUT2Services.Infra.Security.Interfaces;

public interface IAccountServiceApp
{
    Task<CommandResponse> LoginAsync(LoginViewModel viewModel);

    Task<CommandResponse> LoginOrganizacionAsync(LoginOrganizacionViewModel viewModel);

    Task<CommandResponse> RefreshTokenAsync(RefreshTokenViewModel viewModel);

    Task<CommandResponse> RegisterAsync(RegisterViewModel viewModel);

    Task<CommandResponse> Logout();

    IEnumerable<string> ProcesosAutorizados();
    IEnumerable<string> RolesPorProceso(string proceso);
    DatosUsuarioDTO DatosUsuario();
}
