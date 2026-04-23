// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.128
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Handlers;

public partial class ValidacionEnrrolamientoCommandHandler : CommandHandler
{
    private readonly IValidacionEnrrolamientoRepository _validacionEnrrolamientoRepository;
    private readonly IClock _clock;

    public ValidacionEnrrolamientoCommandHandler(
        IValidacionEnrrolamientoRepository validacionEnrrolamientoRepository,
        IClock clock,
        IAuditBuffer auditBuffer)
    {
        _validacionEnrrolamientoRepository = validacionEnrrolamientoRepository ?? throw new ArgumentNullException(nameof(validacionEnrrolamientoRepository));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        SetAuditBuffer(auditBuffer);
    }
}
