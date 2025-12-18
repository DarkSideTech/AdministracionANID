using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AUT2Services.Infra.Security.Accounts.Login;

public class LoginCommandHandler : CommandHandler,
    IRequestHandler<LoginCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly ITokenService tokenService;
    private readonly JwtOptions jwtOptions;

    public LoginCommandHandler(
        UserManager<Usuario> userManager,
        ITokenService tokenService,
        IOptions<JwtOptions> jwtOptions
        )
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
        this.jwtOptions = jwtOptions.Value;
    }

    public async Task<CommandResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
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
            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.Email == command.Email!);

            if (user is null)
            {
                AddError("Usuario o clave no corresponden");
                return CommandResponse;
            }

            var resultado = await userManager
                .CheckPasswordAsync(user, command.Password!);

            if (!resultado)
            {
                AddError("Usuario o clave no corresponden");
                return CommandResponse;
            }

            var (jwtToken, expirationDateInUtc) = tokenService.GenerateJwtTokenLogin(user);
            var refreshTokenValue = tokenService.GenerateRefreshToken();

            var refreshTokenExpirationDateInUtc = DateTime.UtcNow.AddMinutes(this.jwtOptions.ExpirationRefreshTokenTimeInMinutes);

            user.RefreshToken = refreshTokenValue;
            user.RefreshTokenExpiresAtUtc = refreshTokenExpirationDateInUtc;

            await userManager.UpdateAsync(user);

            tokenService.WriteAuthTokenAsHttpOnlyCookie(EnumAuthCookie.ACCESS_TOKEN, jwtToken, expirationDateInUtc);
            tokenService.WriteAuthTokenAsHttpOnlyCookie(EnumAuthCookie.REFRESH_TOKEN, user.RefreshToken, refreshTokenExpirationDateInUtc);

            profile = new ProfileModel
            {
                Token = jwtToken,
                RefreshToken = refreshTokenValue,
            };

        }
        catch (Exception ex)
        {
            AddError($"Error al momento de obtener los datos del usuario, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
        }

        CommandResponse.Data = JsonConvert.SerializeObject(profile);
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
