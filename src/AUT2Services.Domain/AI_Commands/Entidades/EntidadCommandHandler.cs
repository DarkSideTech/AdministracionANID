// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.135
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Entidades.Handlers;

public partial class EntidadCommandHandler : CommandHandler
{
    private readonly IEntidadRepository _entidadRepository;
    private readonly IClock _clock;

    public EntidadCommandHandler(
        IEntidadRepository entidadRepository,
        IClock clock,
        IAuditBuffer auditBuffer)
    {
        _entidadRepository = entidadRepository ?? throw new ArgumentNullException(nameof(entidadRepository));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        SetAuditBuffer(auditBuffer);
    }
}
