using FluentValidation.Results;
using AUT2Services.Domain.Core.Data;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Domain.Core.Commands;

public abstract class CommandHandler
{
    protected CommandResponse CommandResponse;

    protected CommandHandler()
    {
        CommandResponse = new CommandResponse();
    }

    protected void AddError(string mensagem)
    {
        CommandResponse.ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

    protected void AddError<T>(string mensagem, ILogger<T> _logger)
    {
        CommandResponse.ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        _logger.LogError(mensagem);
    }

    protected async Task<CommandResponse> Commit(IUnitOfWork uow, string message)
    {
        if (!await uow.Commit()) AddError(message);

        return CommandResponse;
    }

    protected async Task<CommandResponse> Commit(IUnitOfWork uow)
    {
        return await Commit(uow, "There was an error saving data").ConfigureAwait(false);
    }
}
