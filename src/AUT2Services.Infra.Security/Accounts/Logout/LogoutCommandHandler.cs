using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Security.Accounts.Logout;

public class LogoutCommandHandler : CommandHandler,
    IRequestHandler<LogoutCommand, CommandResponse>
{
    private readonly UserManager<Usuario> userManager;
    private readonly ITokenService tokenService;
    private readonly IUserAccessor userAccessor;

    public LogoutCommandHandler(
        UserManager<Usuario> userManager,
        ITokenService tokenService,
        IUserAccessor userAccessor)
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
        this.userAccessor = userAccessor;
    }

    public async Task<CommandResponse> Handle(LogoutCommand command, CancellationToken cancellationToken)
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
            tokenService.DeleteAuthCookie(EnumAuthCookie.ACCESS_TOKEN);
            tokenService.DeleteAuthCookie(EnumAuthCookie.REFRESH_TOKEN);

            var user = await userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

            if (user is null)
            {
                AddError("Usuario no se encuentra logueado");
                return CommandResponse;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiresAtUtc = null;

            await userManager.UpdateAsync(user);
        }
        catch (Exception ex)
        {
            AddError($"No es posible generar un logout completo para el usuario, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
        }

        CommandResponse.Result = true;
        return CommandResponse;
    }
}
