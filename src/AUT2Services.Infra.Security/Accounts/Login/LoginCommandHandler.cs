using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Records;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AUT2Services.Infra.Security.Accounts.Login;

public class LoginCommandHandler(
    UserManager<Usuario> userManager,
    SignInManager<Usuario> signInManager,
    ITokenService tokenService,
    AUT2ServicesContext aUT2ServicesContext,
    IAuthCookieService authCookieService,
    ICsrfService csrfService,
    IEntidadRepository entidadRepository
    ) : CommandHandler,
    IRequestHandler<LoginCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager = userManager;
    private readonly SignInManager<Usuario> signInManager = signInManager;
    private readonly ITokenService tokenService = tokenService;
    private readonly AUT2ServicesContext aUT2ServicesContext = aUT2ServicesContext;
    private readonly IAuthCookieService authCookieService = authCookieService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly IEntidadRepository entidadRepository = entidadRepository;

    public async Task<CommandResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        AccessTokenResult? accessTokenResult = null;
        Entidad? entidad = null;
        Usuario? usuario = null;

        try
        {
            if (!csrfService.IsRequestValid(command.Request))
            {
                AddError("Invalid CSRF token.");
                return CommandResponse;
            }

            var user = await userManager.FindByEmailAsync(command.Email!);
            if (user is null)
            {
                AddError("Credenciales no válidas.");
                return CommandResponse;
            }
            else
            {
                usuario = user;
            }

            var result = await signInManager.CheckPasswordSignInAsync(usuario, command.Password!, false);
            if (result.IsNotAllowed)
            {
                AddError("Debes confirmar tu correo electrónico antes de iniciar sesión.");
                return CommandResponse;
            }
            if (!result.Succeeded)
            {
                AddError("Credenciales no válidas.");
                return CommandResponse;
            }

            entidad = await entidadRepository.BuscarPor_Id_Usuario_TipoDeEntidad_Persona(Guid.Parse(usuario.Id));

            if (entidad is null)
            {
                AddError($"La entidad asociada al usuario no existe");
                return CommandResponse;
            }

            var sessionId = Guid.NewGuid().ToString("N");
            accessTokenResult = await tokenService.GenerateAccessTokenAsync(usuario, sessionId, entidad.Id);
            var refreshToken = tokenService.CreateRefreshToken(sessionId);
            refreshToken.RefreshToken.UserId = usuario.Id;

            aUT2ServicesContext.RefreshTokens.Add(refreshToken.RefreshToken);
            await aUT2ServicesContext.SaveChangesAsync(cancellationToken);

            authCookieService.AppendAuthCookies(
                command.Response,
                accessTokenResult, 
                refreshToken.RefreshToken.ExpiresAtUtc, 
                refreshToken.PlainTextToken
            );
            csrfService.EnsureTokenCookie(command.Response);
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
        }

        if (usuario is null)
        {
            AddError("Usuario o clave no corresponden");
            return CommandResponse;
        }

        CommandResponse.Data = JsonConvert.SerializeObject(new ProfileLogin(
            AccessTokenExpiracion: accessTokenResult!.ExpiresAtUtc,
            OrganizacionesPorUsuario: await tokenService.BuscarOrganizacionesPorIdUsuario(usuario.Id),
            User: await tokenService.CreateUserDtoAsync(usuario, entidad!.Id),
            ProcesosActivos: null,
            CodigoOrganizacionSeleccionada: null,
            IdEntidadSeleccionada: null,
            SeleccionOrganizacionRequerida: true
        ));
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
