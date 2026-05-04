using AUT2Services.Infra.Security.Accounts.CambioUnidadOrganizacionalEntidadRol;
using AUT2Services.Infra.Security.Accounts.ConfirmaCambioClave;
using AUT2Services.Infra.Security.Accounts.ConfirmaRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.EmailConfirmationToken;
using AUT2Services.Infra.Security.Accounts.Login;
using AUT2Services.Infra.Security.Accounts.LoginClaveUnica;
using AUT2Services.Infra.Security.Accounts.LoginOrganizacion;
using AUT2Services.Infra.Security.Accounts.BuscarUsuariosPaginados;
using AUT2Services.Infra.Security.Accounts.ActivarUsuario;
using AUT2Services.Infra.Security.Accounts.AdminModificaCorreoElectronico;
using AUT2Services.Infra.Security.Accounts.DesactivarUsuario;
using AUT2Services.Infra.Security.Accounts.ModificaCorreoElectronico;
using AUT2Services.Infra.Security.Accounts.ModificaUsuario;
using AUT2Services.Infra.Security.Accounts.RefreshToken;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoCambioClave;
using AUT2Services.Infra.Security.Accounts.ReenviaCodigoRecuperacionClave;
using AUT2Services.Infra.Security.Accounts.Register;
using AUT2Services.Infra.Security.Accounts.ResendEmailConfirmationToken;
using AUT2Services.Infra.Security.Accounts.Roles;
using AUT2Services.Infra.Security.Accounts.SolicitaCambioClave;
using AUT2Services.Infra.Security.Accounts.SolicitaRecuperacionClave;
using AUT2Services.Infra.Security.Records;
using AUT2Services.Infra.Security.ViewModels;
using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Extensions;

public static class AccountExtensions
{
    public static LoginCommand ToLoginCommand(this LoginViewModel viewModel, HttpRequest request, HttpResponse response)
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

    public static CambioUnidadOrganizacionalEntidadRolCommand ToCambioUnidadOrganizacionalEntidadRolCommand(this CambioUnidadOrganizacionalEntidadRolViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        if (viewModel is null) return null;

        return new CambioUnidadOrganizacionalEntidadRolCommand()
        {
            Id_Entidad = viewModel.Id_Entidad,
            Id_Rol = viewModel.Id_Rol,
            Request = request,
            Response = response,
            Context = httpContext
        };
    }

    public static LoginClaveUnicaCommand ToLoginClaveUnicaCommand(this LoginClaveUnicaViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new LoginClaveUnicaCommand()
        {
            ClientId = viewModel.ClientId,
            RedirectUri = viewModel.RedirectUri,
            Code = viewModel.Code,
            State = viewModel.State,
            Request = request,
            Response = response
        };
    }

    public static RegisterCommand ToRegisterCommand(this RegisterViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new RegisterCommand()
        {
            CorreoElectronico = viewModel.CorreoElectronico,
            NumeroDeTelefono = viewModel.NumeroDeTelefono,
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

    public static ModificaUsuarioCommand ToModificaUsuarioCommand(this ModificaUsuarioViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ModificaUsuarioCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            CorreoElectronico = viewModel.CorreoElectronico,
            NumeroDeTelefono = viewModel.NumeroDeTelefono,
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

    public static BuscarUsuariosPaginadosCommand ToBuscarUsuariosPaginadosCommand(this BuscarUsuariosPaginadosViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        if (viewModel is null) return null;

        return new BuscarUsuariosPaginadosCommand()
        {
            NumeroDePagina = viewModel.NumeroDePagina,
            CantidadPorPagina = viewModel.CantidadPorPagina,
            Busqueda = viewModel.Busqueda,
            Request = request,
            Response = response,
            Context = httpContext
        };
    }

    public static ActivarUsuarioCommand ToActivarUsuarioCommand(this ActivarUsuarioViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ActivarUsuarioCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            Request = request,
            Response = response
        };
    }

    public static DesactivarUsuarioCommand ToDesactivarUsuarioCommand(this DesactivarUsuarioViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new DesactivarUsuarioCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            Request = request,
            Response = response
        };
    }

    public static AdminModificaCorreoElectronicoCommand ToAdminModificaCorreoElectronicoCommand(this AdminModificaCorreoElectronicoViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new AdminModificaCorreoElectronicoCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            NuevoCorreoElectronico = viewModel.NuevoCorreoElectronico,
            Request = request,
            Response = response
        };
    }

    public static ModificaCorreoElectronicoCommand ToModificaCorreoElectronicoCommand(this ModificaCorreoElectronicoViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ModificaCorreoElectronicoCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            NuevoCorreoElectronico = viewModel.NuevoCorreoElectronico,
            Request = request,
            Response = response
        };
    }

    public static SolicitaCambioClaveCommand ToSolicitaCambioClaveCommand(this SolicitaCambioClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new SolicitaCambioClaveCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            ClaveActual = viewModel.ClaveActual,
            NuevaClave = viewModel.NuevaClave,
            ConfirmaNuevaClave = viewModel.ConfirmaNuevaClave,
            Request = request,
            Response = response
        };
    }

    public static ReenviaCodigoCambioClaveCommand ToReenviaCodigoCambioClaveCommand(this ReenviaCodigoCambioClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ReenviaCodigoCambioClaveCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            Request = request,
            Response = response
        };
    }

    public static ConfirmaCambioClaveCommand ToConfirmaCambioClaveCommand(this ConfirmaCambioClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ConfirmaCambioClaveCommand()
        {
            IdUsuario = viewModel.IdUsuario,
            ClaveActual = viewModel.ClaveActual,
            NuevaClave = viewModel.NuevaClave,
            ConfirmaNuevaClave = viewModel.ConfirmaNuevaClave,
            CodigoValidacion = viewModel.CodigoValidacion,
            Request = request,
            Response = response
        };
    }

    public static SolicitaRecuperacionClaveCommand ToSolicitaRecuperacionClaveCommand(this SolicitaRecuperacionClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new SolicitaRecuperacionClaveCommand()
        {
            CorreoElectronico = viewModel.CorreoElectronico,
            Request = request,
            Response = response
        };
    }

    public static ReenviaCodigoRecuperacionClaveCommand ToReenviaCodigoRecuperacionClaveCommand(this ReenviaCodigoRecuperacionClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ReenviaCodigoRecuperacionClaveCommand()
        {
            CorreoElectronico = viewModel.CorreoElectronico,
            Request = request,
            Response = response
        };
    }

    public static ConfirmaRecuperacionClaveCommand ToConfirmaRecuperacionClaveCommand(this ConfirmaRecuperacionClaveViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ConfirmaRecuperacionClaveCommand()
        {
            CorreoElectronico = viewModel.CorreoElectronico,
            CodigoValidacion = viewModel.CodigoValidacion,
            NuevaClave = viewModel.NuevaClave,
            ConfirmaNuevaClave = viewModel.ConfirmaNuevaClave,
            Request = request,
            Response = response
        };
    }

    public static BuscarRolesPaginadosCommand ToBuscarRolesPaginadosCommand(this BuscarRolesPaginadosViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        if (viewModel is null) return null;

        return new BuscarRolesPaginadosCommand()
        {
            NumeroDePagina = viewModel.NumeroDePagina,
            CantidadPorPagina = viewModel.CantidadPorPagina,
            Busqueda = viewModel.Busqueda,
            Request = request,
            Response = response,
            Context = httpContext
        };
    }

    public static BuscarRolesCommand ToBuscarRolesCommand(this BuscarRolesViewModel viewModel, HttpRequest request, HttpResponse response, HttpContext httpContext)
    {
        if (viewModel is null) return null;

        return new BuscarRolesCommand()
        {
            Estado = viewModel.Estado,
            Request = request,
            Response = response,
            Context = httpContext
        };
    }

    public static ModificaRolCommand ToModificaRolCommand(this ModificaRolViewModel viewModel, HttpRequest request, HttpResponse response)
    {
        if (viewModel is null) return null;

        return new ModificaRolCommand()
        {
            IdRol = viewModel.IdRol,
            Descripcion = viewModel.Descripcion,
            ValidaEnrrolamiento = viewModel.ValidaEnrrolamiento,
            ValidaAsignacionDeRoles = viewModel.ValidaAsignacionDeRoles,
            Request = request,
            Response = response
        };
    }

    public static ActivarRolCommand ToActivarRolCommand(this ActivarRolViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static DesactivarRolCommand ToDesactivarRolCommand(this DesactivarRolViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static RequiereValidacionAlSerAsignadoCommand ToRequiereValidacionAlSerAsignadoCommand(this RequiereValidacionAlSerAsignadoViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static NoRequiereValidacionAlSerAsignadoCommand ToNoRequiereValidacionAlSerAsignadoCommand(this NoRequiereValidacionAlSerAsignadoViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static ActivaValidacionDeAsignacionDeRolesCommand ToActivaValidacionDeAsignacionDeRolesCommand(this ActivaValidacionDeAsignacionDeRolesViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static DesactivaValidacionDeAsignacionDeRolesCommand ToDesactivaValidacionDeAsignacionDeRolesCommand(this DesactivaValidacionDeAsignacionDeRolesViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static ActivaDetalleDeAutorizacionesCommand ToActivaDetalleDeAutorizacionesCommand(this ActivaDetalleDeAutorizacionesViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };

    public static DesactivaDetalleDeAutorizacionesCommand ToDesactivaDetalleDeAutorizacionesCommand(this DesactivaDetalleDeAutorizacionesViewModel viewModel, HttpRequest request, HttpResponse response)
        => new()
        {
            IdRol = viewModel?.IdRol,
            Request = request,
            Response = response
        };
}
