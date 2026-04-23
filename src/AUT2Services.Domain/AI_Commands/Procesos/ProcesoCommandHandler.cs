// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.152
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Procesos.Handlers;

public partial class ProcesoCommandHandler : CommandHandler
{
    private readonly IProcesoRepository _procesoRepository;

    public ProcesoCommandHandler(
        IProcesoRepository procesoRepository,
        IAuditBuffer auditBuffer)
    {
        _procesoRepository = procesoRepository ?? throw new ArgumentNullException(nameof(procesoRepository));
        SetAuditBuffer(auditBuffer);
    }
}

