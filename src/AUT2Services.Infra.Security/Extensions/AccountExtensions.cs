using AUT2Services.Infra.Security.Accounts.Login;
using AUT2Services.Infra.Security.Accounts.LoginOrganizacion;
using AUT2Services.Infra.Security.Accounts.RefreshToken;
using AUT2Services.Infra.Security.Accounts.Register;
using AUT2Services.Infra.Security.Accounts.ReSendEmailConfirmation;
using AUT2Services.Infra.Security.Accounts.ValidateEmail;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Extensions;

public static class AccountExtensions
{
    public static LoginCommand ToLoginCommand(this LoginViewModel viewModel,HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new LoginCommand()
        {
            Email = viewModel.Email,
            Password = viewModel.Password,
            Request = request,
            Response = response
        };
    }

    public static LoginOrganizacionCommand ToLoginOrganizacionCommand(this LoginOrganizacionViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        if (viewModel is null) return null;

        return new LoginOrganizacionCommand()
        {
            Organizacion = viewModel.Organizacion,
            Request = request,
            Response = response,
            Context = httpContext
        };
    }

    public static RegisterCommand ToRegisterCommand(this RegisterViewModel viewModel, HttpRequest request, HttpResponse response)
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
            ConfirmaContraseña = viewModel.ConfirmaContraseña,
            TerminosYCondiciones = viewModel.TerminosYCondiciones,
            Request = request,
            Response = response
        };
    }

    public static RefreshTokenCommand ToRefreshTokenCommand(this RefreshTokenViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new RefreshTokenCommand() 
        { 
            Request = request,
            Response = response
        };
    }

    public static EmailConfirmationTokenCommand ToEmailConfirmationTokenCommand(this ConfirmEmailRequest requestRecord, HttpRequest request, HttpResponse response)
    {
        if (requestRecord is null) return null;

        return new EmailConfirmationTokenCommand()
        {
            UserId = requestRecord.UserId,
            Token = requestRecord.Token,
            Request = request,
            Response = response
        };
    }

    public static ResendEmailConfirmationTokenCommand ToResendEmailConfirmationTokenCommand(this ResendEmailConfirmationTokenRequest requestRecord, HttpRequest request, HttpResponse response)
    {
        if (requestRecord is null) return null;

        return new ResendEmailConfirmationTokenCommand()
        {
            Email = requestRecord.Email,
            Request = request,
            Response = response
        };
    }
}