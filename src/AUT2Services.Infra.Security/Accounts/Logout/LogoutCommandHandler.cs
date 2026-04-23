using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace AUT2Services.Infra.Security.Accounts.Logout;

public class LogoutCommandHandler(
    ITokenService tokenService,
    IAuthCookieService authCookieService,
    ICsrfService csrfService,
    AUT2ServicesContext aUT2ServicesContext,
    ICurrentUserService currentUserService) : CommandHandler,
    IRequestHandler<LogoutCommand, CommandResponse>
{
    private readonly ITokenService tokenService = tokenService;
    private readonly IAuthCookieService authCookieService = authCookieService;
    private readonly ICsrfService csrfService = csrfService;
    private readonly AUT2ServicesContext aUT2ServicesContext = aUT2ServicesContext;
    private readonly ICurrentUserService currentUserService = currentUserService;

    public async Task<CommandResponse> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        try
        {
            if (!csrfService.IsRequestValid(command.Request))
            {
                AddError("Invalid CSRF token.");
                return CommandResponse;
            }

            if (command.Request.Cookies.TryGetValue(EnumAuthCookieNames.RefreshToken, out var tokenValue))
            {
                var tokenHash = tokenService.HashRefreshToken(tokenValue);
                var refreshToken = await aUT2ServicesContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
                if (refreshToken is not null)
                {
                    await tokenService.RevokeSessionAsync(refreshToken.SessionId, EnumRefreshTokenRevocationReasons.Logout);
                }
            }
            else
            {
                var claimsPrincipal = currentUserService.GetClaimsPrincipal();
                if (claimsPrincipal is null)
                {
                    AddError("Usuario no esta logueado.");
                    return CommandResponse;
                }

                var sessionId = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    await tokenService.RevokeSessionAsync(sessionId, EnumRefreshTokenRevocationReasons.Logout);
                }
            }
        }
        catch (Exception ex)
        {
            AddError($"No es posible generar un logout completo para el usuario, message [{ex.Message}]");
            CommandResponse.Data = string.Empty;
            CommandResponse.Result = false;
        }
        finally
        {
            authCookieService.ClearAuthCookies(command.Response);
            command.Request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var existingToken);
            csrfService.EnsureTokenCookie(command.Response, existingToken);
        }

        CommandResponse.Result = true;
        return CommandResponse;
    }
}
