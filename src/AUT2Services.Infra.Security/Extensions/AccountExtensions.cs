using AUT2Services.Infra.Security.Accounts.Login;
using AUT2Services.Infra.Security.Accounts.LoginOrganizacion;
using AUT2Services.Infra.Security.Accounts.RefreshToken;
using AUT2Services.Infra.Security.Accounts.Register;
using AUT2Services.Infra.Security.ViewModels;

namespace AUT2Services.Infra.Security.Extensions;

public static class AccountExtensions
{
    public static LoginCommand ToLoginCommand(this LoginViewModel viewModel)
    {
        if (viewModel is null) return null;

        return new LoginCommand()
        {
            Email = viewModel.Email,
            Password = viewModel.Password
        };
    }

    public static LoginOrganizacionCommand ToLoginOrganizacionCommand(this LoginOrganizacionViewModel viewModel)
    {
        if (viewModel is null) return null;

        return new LoginOrganizacionCommand()
        {
            Email = viewModel.Email,
            Password = viewModel.Password,
            Organizacion = viewModel.Organizacion
        };
    }

    public static RegisterCommand ToLoginOrganizacionCommand(this RegisterViewModel viewModel)
    {
        if (viewModel is null) return null;

        return new RegisterCommand()
        {
            CorreoElectronico = viewModel.CorreoElectronico,
            Nacionalidad = viewModel.Nacionalidad,
            TipoDeUsuario = viewModel.TipoDeUsuario,
            DocumentoDeIdentidad = viewModel.DocumentoDeIdentidad,
            NumeroDeDocumento = viewModel.NumeroDeDocumento,
            CodigoValidadorDocumento = viewModel.CodigoValidadorDocumento,
            PrimerNombre = viewModel.PrimerNombre,
            SegundoNombre = viewModel.SegundoNombre,
            PrimerApellido = viewModel.PrimerApellido,
            SegundoApellido = viewModel.SegundoApellido,
            SexoDeclarativo = viewModel.SexoDeclarativo,
            SexoRegistral = viewModel.SexoRegistral,
            FechaDeNacimiento = viewModel.FechaDeNacimiento,
            Contraseña = viewModel.Contraseña,
            TerminosYCondiciones = viewModel.TerminosYCondiciones
        };
    }

    public static RefreshTokenCommand ToRefreshTokenCommand(this RefreshTokenViewModel viewModel)
    {
        if (viewModel is null) return null;

        return new RefreshTokenCommand() 
        { 
            RefreshToken = viewModel.RefreshToken 
        };
    }

}