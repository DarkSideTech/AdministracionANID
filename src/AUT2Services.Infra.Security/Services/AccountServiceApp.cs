using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.DTOs;
using AUT2Services.Infra.Security.Accounts.Logout;
using AUT2Services.Infra.Security.Extensions;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.ViewModels;

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

    public Task<CommandResponse> LoginAsync(LoginViewModel viewModel)
    {
        return mediator.SendCommand(viewModel.ToLoginCommand());
    }

    public Task<CommandResponse> LoginOrganizacionAsync(LoginOrganizacionViewModel viewModel)
    {
        return mediator.SendCommand(viewModel.ToLoginOrganizacionCommand());
    }

    public Task<CommandResponse> RefreshTokenAsync(RefreshTokenViewModel viewModel)
    {
        return mediator.SendCommand(viewModel.ToRefreshTokenCommand());
    }

    public Task<CommandResponse> RegisterAsync(RegisterViewModel viewModel)
    {
        Task<CommandResponse> result = null!;
        try
        {
            result = mediator.SendCommand(viewModel.ToRegisterCommand());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return result;
    }

    public async Task<CommandResponse> Logout()
    {
        return await mediator.SendCommand(new LogoutCommand());
    }

    public IEnumerable<string> ProcesosAutorizados()
    {
        return userAccessor.GetProcesos();
    }

    public IEnumerable<string> RolesPorProceso(string proceso)
    {
        return userAccessor.GetRolesPorProceso(proceso);
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
