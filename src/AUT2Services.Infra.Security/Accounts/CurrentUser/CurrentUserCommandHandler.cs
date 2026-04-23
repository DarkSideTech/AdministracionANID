using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Accounts.CurrentUser;

public class CurrentUserCommandHandler(
    ICsrfService csrfService,
    ICurrentUserService currentUserService) : CommandHandler,
    IRequestHandler<CurrentUserCommand, CommandResponse>
{
    private readonly ICsrfService csrfService = csrfService;
    private readonly ICurrentUserService currentUserService = currentUserService;

    public async Task<CommandResponse> Handle(CurrentUserCommand command, CancellationToken cancellationToken)
    {
        CommandResponse = command.CommandResponse;
        CommandResponse.Result = false;

        if (!command.IsValid())
        {
            return CommandResponse;
        }

        command.Request.Cookies.TryGetValue(EnumCsrfNames.Cookie, out var existingToken);
        csrfService.EnsureTokenCookie(command.Response, existingToken);

        CommandResponse.Data = await currentUserService.GetCurrentUserResponseAsync(command.Context.RequestAborted);
        CommandResponse.Result = true;

        return CommandResponse;
    }
}
