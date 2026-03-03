using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.Logout;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace AUT2Services.Infra.Security.Accounts.RefreshToken;

public class RefreshTokenCommandHandler : CommandHandler,
    IRequestHandler<RefreshTokenCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly ITokenService tokenService;
    private readonly ISecurityRepository securityRepository;
    private readonly IUserAccessor userAccessor;
    private readonly IMediatorHandler mediator;

    public RefreshTokenCommandHandler(
        UserManager<Usuario> userManager,
        ITokenService tokenService,
        ISecurityRepository securityRepository,
        IUserAccessor userAccessor,
        IMediatorHandler mediator
        )
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
        this.securityRepository = securityRepository;
        this.userAccessor = userAccessor;
        this.mediator = mediator;
    }

    public async Task<CommandResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }
        var profile = new ProfileModel();

        try
        {
            var user = await securityRepository.BuscarUsuarioPor_RefreshToken(command.RefreshToken!);

            if (user is null)
            {
                AddError("Falta el token de actualización.");
                return CommandResponse;
            }

            if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow)
            {
                AddError("El token de actualización ha expirado.");

                var logoutCommand = new LogoutCommand() {};
                var commandResult = await mediator.SendCommand(logoutCommand);
                if (!commandResult.Result)
                {
                    CommandResponse.ValidationResult.Errors.AddRange(commandResult.ValidationResult.Errors);
                }
                return CommandResponse;
            }

            string jwtToken = string.Empty;
            DateTime expirationDateInUtc = DateTime.MinValue;

            switch (userAccessor.GetAccessTokenType())
            {
                case EnumAccessTokenType.LOGIN:
                    (jwtToken, expirationDateInUtc) = tokenService.GenerateJwtTokenLogin(user);
                    break;
                case EnumAccessTokenType.LOGIN_ORGANIZATION:
                    (jwtToken, expirationDateInUtc) = await tokenService.GenerateJwtTokenLoginOrganizacion(user, Guid.Parse(userAccessor.GetIdEntidad()));
                    break;
                default:
                    AddError("la informacion del usuario logueado no esta disponible");
                    var logoutCommand = new LogoutCommand() { };
                    var commandResult = await mediator.SendCommand(logoutCommand);
                    if (!commandResult.Result)
                    {
                        CommandResponse.ValidationResult.Errors.AddRange(commandResult.ValidationResult.Errors);
                    }
                    return CommandResponse;
            }

            user.RefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiresAtUtc = (DateTime)user.RefreshTokenExpiresAtUtc!;

            await userManager.UpdateAsync(user);

            tokenService.WriteAuthTokenAsHttpOnlyCookie(EnumAuthCookie.ACCESS_TOKEN, jwtToken, expirationDateInUtc);
            tokenService.WriteAuthTokenAsHttpOnlyCookie(EnumAuthCookie.REFRESH_TOKEN, user.RefreshToken, (DateTime)user.RefreshTokenExpiresAtUtc);

            profile = new ProfileModel
            {
                AccessToken = jwtToken,
                RefreshToken = user.RefreshToken,
            };
        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            var logoutCommand = new LogoutCommand() { };
            var commandResult = await mediator.SendCommand(logoutCommand);
            if (!commandResult.Result)
            {
                CommandResponse.ValidationResult.Errors.AddRange(commandResult.ValidationResult.Errors);
            }
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
        }

        CommandResponse.Data = JsonConvert.SerializeObject(profile);
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
