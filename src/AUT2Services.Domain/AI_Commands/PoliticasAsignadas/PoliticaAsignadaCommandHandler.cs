// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.146
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Time;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;

public partial class PoliticaAsignadaCommandHandler : CommandHandler
{
    private readonly IPoliticaAsignadaRepository _politicaAsignadaRepository;
    private readonly IClock _clock;

    public PoliticaAsignadaCommandHandler(
        IPoliticaAsignadaRepository politicaAsignadaRepository,
        IClock clock,
        IAuditBuffer auditBuffer)
    {
        _politicaAsignadaRepository = politicaAsignadaRepository ?? throw new ArgumentNullException(nameof(politicaAsignadaRepository));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        SetAuditBuffer(auditBuffer);
    }
}
