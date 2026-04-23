using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Core.Domain;
using AUT2Services.Domain.Core.Events;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AUT2Services.Domain.Core.Commands;

public abstract class CommandHandler
{
    protected CommandResponse CommandResponse;
    protected IAuditBuffer AuditBuffer { get; private set; }

    protected CommandHandler()
    {
        CommandResponse = new CommandResponse();
        AuditBuffer = NoOpAuditBuffer.Instance;
    }

    protected void SetAuditBuffer(IAuditBuffer auditBuffer)
    {
        AuditBuffer = auditBuffer ?? NoOpAuditBuffer.Instance;
    }

    protected void AddError(string mensagem)
    {
        CommandResponse.ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
    }

    protected void AddError<T>(string mensagem, ILogger<T> logger)
    {
        CommandResponse.ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        logger.LogError(mensagem);
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

    protected void AddCreateDomainEvent<TAggregate, TEvent>(Command command, Entity aggregate, TEvent domainEvent, TAggregate after)
        where TEvent : Event
    {
        aggregate.AddDomainEvent(domainEvent);
        AuditBuffer.TrackCreate(command, domainEvent, after);
    }

    protected void AddUpdateDomainEvent<TAggregate, TEvent>(Command command, Entity aggregate, TEvent domainEvent, TAggregate before, TAggregate after)
        where TEvent : Event
    {
        aggregate.AddDomainEvent(domainEvent);
        AuditBuffer.TrackUpdate(command, domainEvent, before, after);
    }

    protected void AddDeleteDomainEvent<TAggregate, TEvent>(Command command, Entity aggregate, TEvent domainEvent, TAggregate before)
        where TEvent : Event
    {
        aggregate.AddDomainEvent(domainEvent);
        AuditBuffer.TrackDelete(command, domainEvent, before);
    }
}
